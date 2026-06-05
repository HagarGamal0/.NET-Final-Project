using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PickGo_backend;
using PickGo_backend.Helpers;
using PickGo_backend.Models;
using PickGo_backend.Services;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class CourierTrackingService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public CourierTrackingService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var unitOfWork = scope.ServiceProvider.GetRequiredService<UnitOfWork>();
                var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<CourierLocationHub>>();
                var notificationService = scope.ServiceProvider.GetRequiredService<OrderNotificationService>();

                // جميع الكوريرز الاونلاين
                var couriers = (await unitOfWork.CourierRepo.GetAllWithLocationsAsync())
                                .Where(c => c.IsOnline)
                                .ToList();

                foreach (var courier in couriers)
                {
                    var lastLoc = courier.Locations?.OrderByDescending(l => l.RecordedAt).FirstOrDefault();
                    if (lastLoc == null) continue;

                    // جميع الـ Packages اللي assigned للكورير ولم يتم توصيلها
                    var packages = courier.Packages?
                        .Where(p => p.Status != PackageStatus.Delivered && p.Status != PackageStatus.Cancelled)
                        .ToList();

                    if (packages == null || !packages.Any()) continue;

                    foreach (var package in packages)
                    {
                        // حساب المسافة للمكان اللي المفروض يوصله
                        double distanceToPickup = GeoHelper.DistanceKm(
                            package.Request.PickupLat,
                            package.Request.PickupLng,
                            lastLoc.Lat,
                            lastLoc.Lng
                        );

                        // اذا قرب من الـ Pickup (< 1km)
                        if (distanceToPickup <= 1 && package.Status == PackageStatus.Pending)
                        {
                            package.Status = PackageStatus.PickupInProgress;
                            await hubContext.Clients.Group($"order-{package.RequestID}")
                                .SendAsync("ReceiveNotification", new
                                {
                                    title = "Courier Near Pickup 🚴",
                                    message = $"Courier for your request {package.RequestID} is near the pickup location",
                                    createdAt = DateTime.UtcNow
                                });

                            // Email Notification للـ Supplier
                            await notificationService.SendEmailNotification(
                                package.Request.Supplier.User.Email,
                                "Courier Near Pickup 🚴",
                                $"Courier for your request {package.RequestID} is near the pickup location"
                            );
                        }

                        // اذا وصل للـ Delivery (مثال: قرب أقل من 0.5km من destination)
                        if (package.Destination != null)
                        {
                            double distanceToDelivery = GeoHelper.DistanceKm(
                                lastLoc.Lat,
                                lastLoc.Lng,
                                package.Lat ?? 0,
                                package.Lang ?? 0
                            );

                            if (distanceToDelivery <= 0.5 && package.Status == PackageStatus.PickupInProgress)
                            {
                                package.Status = PackageStatus.Delivered;

                                await hubContext.Clients.Group($"order-{package.RequestID}")
                                    .SendAsync("ReceiveNotification", new
                                    {
                                        title = "Package Delivered ✅",
                                        message = $"Package {package.Id} has been delivered",
                                        createdAt = DateTime.UtcNow
                                    });

                                // Email Notification للـ Customer
                                await notificationService.SendEmailNotification(
                                    package.Customer.User.Email,
                                    "Package Delivered ✅",
                                    $"Your package {package.Id} has been delivered successfully"
                                );

                                package.DeliveredAt = DateTime.UtcNow;
                            }
                        }

                        // Handle Abandoned Packages (لم يتحرك لأكثر من 30 دقيقة)
                        if ((DateTime.UtcNow - package.CreatedAt).TotalMinutes > 30 && package.Status == PackageStatus.Pending)
                        {
                            package.Status = PackageStatus.Cancelled;

                            await hubContext.Clients.Group($"order-{package.RequestID}")
                                .SendAsync("ReceiveNotification", new
                                {
                                    title = "Package Cancelled ⚠️",
                                    message = $"Package {package.Id} has been cancelled due to inactivity",
                                    createdAt = DateTime.UtcNow
                                });

                            await notificationService.SendEmailNotification(
                                package.Customer.User.Email,
                                "Package Cancelled ⚠️",
                                $"Your package {package.Id} has been cancelled due to inactivity"
                            );
                        }
                    }

                    unitOfWork.CourierRepo.Update(courier);
                }

                await unitOfWork.SaveAsync();
            }

            // الانتظار قبل الدورة التالية (10 ثواني)
            await Task.Delay(10000);
        }
    }
}

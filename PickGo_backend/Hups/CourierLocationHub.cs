using Microsoft.AspNetCore.SignalR;
using PickGo_backend;
using PickGo_backend.Models;
using PickGo_backend.Services;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

public class CourierLocationHub : Hub
{
    private readonly UnitOfWork _unitOfWork;

    public CourierLocationHub(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task SendLocation(double lat, double lng, int orderId)
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return;

        var courier = await _unitOfWork.CourierRepo
            .GetByExpressionAsync(c => c.UserId == userId);

        if (courier == null || !courier.IsOnline) return;

        var location = new CourierLocation
        {
            CourierID = courier.Id,
            Lat = (float)lat,
            Lng = (float)lng,
            RecordedAt = DateTime.UtcNow
        };

        await _unitOfWork.CourierLocationRepo.AddAsync(location);
        await _unitOfWork.SaveAsync();

        await Clients.Group($"order-{orderId}")
            .SendAsync("ReceiveLocation", lat, lng);
    }

    public async Task JoinOrderGroup(int orderId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"order-{orderId}");
    }

    public async Task SendOrderNotification(int orderId, string title, string message)
    {
        await Clients.Group($"order-{orderId}")
            .SendAsync("ReceiveNotification", new
            {
                title,
                message,
                createdAt = DateTime.UtcNow
            });
    }

    public override async Task OnConnectedAsync()
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId != null)
        {
            var courier = await _unitOfWork.CourierRepo
                .GetByExpressionAsync(c => c.UserId == userId);

            if (courier != null)
            {
                var activePackages = courier.Packages?
                    .Where(p => p.Status != PackageStatus.Delivered && p.Status != PackageStatus.Cancelled);

                if (activePackages != null)
                {
                    foreach (var p in activePackages)
                        await Groups.AddToGroupAsync(Context.ConnectionId, $"order-{p.RequestID}");
                }
            }
        }

        await base.OnConnectedAsync();
    }
}

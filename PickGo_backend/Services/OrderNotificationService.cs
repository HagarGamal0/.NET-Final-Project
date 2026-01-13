using Microsoft.AspNetCore.SignalR;

public class OrderNotificationService
{
    private readonly IHubContext<CourierLocationHub> _hubContext;
    private readonly IHubContext<NotificationHub> _notificationHub;

    public OrderNotificationService(IHubContext<CourierLocationHub> hubContext, IHubContext<NotificationHub> notificationHub)
    {
        _hubContext = hubContext;
        _notificationHub = notificationHub;
    }

    public async Task NotifyCourierNearby(int orderId)
    {
        await _hubContext.Clients.Group($"order-{orderId}")
            .SendAsync("ReceiveNotification", new
            {
                title = "Courier is nearby 🚴",
                message = "Courier is arriving soon"
            });
    }
    public async Task NotifyCourierDelivered(int orderId)
    {
        await _hubContext.Clients.Group($"order-{orderId}")
            .SendAsync("ReceiveNotification", new
            {
                title = "Order Delivered ✅",
                message = "Your order has been delivered successfully",
                createdAt = DateTime.UtcNow
            });
    }

    // Email Example (Offline Notification)
    public async Task SendEmailNotification(string email, string subject, string body)
    {
        var message = new MimeKit.MimeMessage();
        message.From.Add(new MimeKit.MailboxAddress("Shipments", "no-reply@shipments.com"));
        message.To.Add(new MimeKit.MailboxAddress("", email));
        message.Subject = subject;
        message.Body = new MimeKit.TextPart("plain") { Text = body };

        using var client = new MailKit.Net.Smtp.SmtpClient();
        await client.ConnectAsync("smtp.gmail.com", 587, false);
        await client.AuthenticateAsync("your_email", "your_password");
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }

public async Task NotifyNewRequest(int requestId)
{
    await _notificationHub.Clients
        .Group("Couriers")
        .SendAsync("NewRequestCreated", new
        {
            requestNumber = requestId,
            message = $"New delivery request #{requestId} available"
        });
}


}

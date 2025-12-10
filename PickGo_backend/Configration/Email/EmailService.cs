using System.Net.Mail;

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var emailSettings = _config.GetSection("EmailSettings");
        var smtp = new SmtpClient
        {
            Host = emailSettings["SmtpServer"],
            Port = int.Parse(emailSettings["SmtpPort"]),
            EnableSsl = true,
            Credentials = new System.Net.NetworkCredential(
                emailSettings["SenderEmail"],
                emailSettings["SenderPassword"]
            )
        };

        var mail = new MailMessage
        {
            From = new MailAddress(emailSettings["SenderEmail"], emailSettings["SenderName"]),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };
        mail.To.Add(toEmail);

        await smtp.SendMailAsync(mail);
    }
}
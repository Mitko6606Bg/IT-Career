using MailKit.Net.Smtp;
using MimeKit;
using System.Net.Mail;
using System.Threading.Tasks;

public class EmailService
{
    public async Task SendReservationEmailAsync(string recipientEmail, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Flight Manager", "flight.manager72024@gmail.com"));
        message.To.Add(new MailboxAddress("", recipientEmail));
        message.Subject = subject;


        var builder = new BodyBuilder();
        builder.HtmlBody = body;
        message.Body = builder.ToMessageBody();

        using (var client = new MailKit.Net.Smtp.SmtpClient())
        {
            await client.ConnectAsync("smtp.gmail.com", 587, false);
            await client.AuthenticateAsync("flight.manager72024@gmail.com", "dgvz volh ctpt iwhx");
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}

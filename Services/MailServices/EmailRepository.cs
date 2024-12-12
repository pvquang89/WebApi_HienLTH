
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using WebApi_HienLTH.Models.MailModel;

namespace WebApi_HienLTH.Services.MailServices
{
    public class EmailRepository : IEmailRepository
    {
        private readonly SmtpSettings _smtpSettings;

        public EmailRepository(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value; //lấy giá trị cấu hình từ appSettings.json
        }



        //phương thức cấu trúc mail và chính thức gửi mail
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var smtpSettings = _smtpSettings;
            //MimeMessage : đối tượng dùng để tạo và cấu trúc mail 
            var emailMessage = new MimeMessage();
            //(tên người gửi,mail người gửi) 
            emailMessage.From.Add(new MailboxAddress("Admin", smtpSettings.From));
            //(tên người nhận,mail người nhận) 
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject; //tiêu đề mail

            // Tạo nội dung HTML cho email
            emailMessage.Body = new TextPart("html") { Text = BuildHtmlEmailBody(message) };

            // Gửi email
            await SendEmailViaSmtp(emailMessage);
        }


        //phương thức kết nối tới smtp server và thực hiện gửi mail
        private async Task SendEmailViaSmtp(MimeMessage emailMessage)
        {
            try
            {
                var smtpSettings = _smtpSettings;

                //tạo obj smtpClient để giao tiếp vưới SMTP server
                //using : tự ngắt kết nối khi không dùng, tức là tự gọi Dispose()
                using var client = new SmtpClient();

                //Kết nối đến smtp
                // Đặt SecureSocketOptions.StartTls cho cổng 587
                await client.ConnectAsync(smtpSettings.Host, smtpSettings.Port, SecureSocketOptions.StartTls);
                //xác thực thông tin người gửi
                await client.AuthenticateAsync(smtpSettings.From, smtpSettings.Password);
                //gửi mail
                await client.SendAsync(emailMessage);
                //ngắt kết nối với máy chủ khi đã xong
                await client.DisconnectAsync(true);

                Console.WriteLine("Email đã được gửi thành công.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi gửi email: {ex.Message}");
            }
        }

        private string BuildHtmlEmailBody(string message)
        {
            return $@"
            <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; }}
                        .email-header {{ font-size: 24px; color: #333; }}
                        .email-content {{ font-size: 16px; color: #555; }}
                        .footer {{ font-size: 12px; color: #999; }}
                    </style>
                </head>
                <body>
                    <div class='email-header'>
                        Chào bạn, 
                    </div>
                    <div class='email-content'>
                        {message}
                        <br>
                        <p>Mã sẽ hết hạn sau 1 giờ !</p>
                    </div>
                    <div class='footer'>
                        <p>Cảm ơn bạn đã sử dụng dịch vụ của chúng tôi.</p>
                        <p>Admin</p>
                    </div>
                </body>
            </html>";
        }


    }
}

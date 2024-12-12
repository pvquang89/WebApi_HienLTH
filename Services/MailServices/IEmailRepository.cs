namespace WebApi_HienLTH.Services.MailServices
{
    public interface IEmailRepository
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}

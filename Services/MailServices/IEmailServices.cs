namespace WebApi_HienLTH.Services.MailServices
{
    public interface IEmailServices
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}

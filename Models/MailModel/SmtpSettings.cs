namespace WebApi_HienLTH.Models.MailModel
{
    //class ánh xạ cấu hình của smtpSettings trong appSettings.json     
    public class SmtpSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool UseSsl { get; set; }
        public string From { get; set; }
        public string Password { get; set; }
    }
}

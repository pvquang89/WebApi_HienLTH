namespace WebApi_HienLTH.Models.JwtModel
{
    //class này để đọc cấu hình từ appSetting.json vào các thuộc tính trong mã nguồn 
    public class JwtSettings
    {
        public string SecretKey { get; set; }
    }
}

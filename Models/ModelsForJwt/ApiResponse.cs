namespace WebApi_HienLTH.Models.ModelsForJwt
{
    //dùng để trả về phản hồi khi validate lúc login
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}

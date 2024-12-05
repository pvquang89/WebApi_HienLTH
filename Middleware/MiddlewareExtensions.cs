namespace WebApi_HienLTH.Middleware
{
    public static class MiddlewareExtensions
    {
        //thêm 1 method mới vào kiểu IApplicationBuilder
        //Từ khóa this cho biết đây là một extension method cho kiểu IApplicationBuilder.
        //app là đối tượng mà phương thức này sẽ mở rộng.
        public static IApplicationBuilder UseCustomForbiddenMiddleware(this IApplicationBuilder app)
        {
            //Đăng ký middleware CustomForbiddenMiddleware vào pipeline của ứng dụng.
            //ASP.NET Core sẽ tự động khởi tạo CustomForbiddenMiddleware
            //và gọi phương thức InvokeAsync khi middleware này được kích hoạt.
            return app.UseMiddleware<CustomForbiddenMiddleware>();
        }
    }
}

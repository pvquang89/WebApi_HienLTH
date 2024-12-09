using System.Text.Json;

namespace WebApi_HienLTH.Middleware
{
    public class CustomForbiddenMiddleware
    {
        //đây là delegate đại diện cho middleware tiếp theo trong pipeline.
        private readonly RequestDelegate _next;
        
        public CustomForbiddenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        //HttpContext : Đại diện cho thông tin của yêu cầu HTTP hiện tại và phản hồi HTTP liên quan.
        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);

            if (context.Response.StatusCode == 403)
            {
                //Đặt kiểu nội dung của phản hồi là JSON.
                context.Response.ContentType = "application/json";
                var response = new
                {
                    message = "Bạn không có quyền truy cập vào tài nguyên này."
                };
                //JsonSerializer.Serialize(response) : tạo ra chuỗi json từ response
                //Ghi dữ liệu (ở đây là chuỗi JSON) vào luồng phản hồi HTTP.
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}

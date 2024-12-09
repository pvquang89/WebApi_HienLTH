namespace WebApi_HienLTH.Models.DTO
{

    //class để hiện thông tin user và quyền tương ứng 
    public class UserWithRolesDto
    {
        public int Id { get; set; }    
        public string UserName { get; set; }
        public string HoTen { get; set; }
        public string Email { get; set; }
        public List<string> VaiTro { get; set; }
    }
}


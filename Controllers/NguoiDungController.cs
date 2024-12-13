using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi_HienLTH.Data;
using WebApi_HienLTH.Models;
using WebApi_HienLTH.Models.DTO;
using WebApi_HienLTH.Repository.NguoiDungRepository;
using WebApi_HienLTH.UnitOfWork;

namespace WebApi_HienLTH.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class NguoiDungController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public NguoiDungController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            return Ok(await _unitOfWork.NguoiDungRepository.GetAllRolesAsync());
        }

        [HttpGet]
        public async Task<IActionResult> GetUserWithRoles()
        {
            var usersWithRoles = await _unitOfWork.NguoiDungRepository.GetAllUserWithRolesAsync();
            return Ok(usersWithRoles);
        }

        [HttpPost]
        public async Task<ActionResult<NguoiDungModel>> CreateUser(NguoiDungModel model)
        {
            await _unitOfWork.NguoiDungRepository.CreateUser(model);
            return Ok(new {message = "User created success !"});
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersGroupedByRole()
        {
            var result = await _unitOfWork.NguoiDungRepository.GetUsersGroupedByRoleAsync();
            return Ok(result);
        }
        //[HttpGet]
        //public async Task<IActionResult> GetUserRoles()
        //{
        //    var userRoles = await _context
        //                 .Set<UserRolesDto>()
        //                 .FromSqlRaw(@"Select *from GetUserRoles()")
        //                 .ToListAsync();
        //    return Ok(userRoles);
        //}

        //[HttpGet]
        //public async Task<IActionResult> GetUsersGroupedByRole()
        //{
        //    // Gọi function GetUserRoles() từ PostgreSQL bằng FromSqlRaw
        //    var users = await _context.Set<UserRolesDto>()
        //        .FromSqlRaw(@"SELECT * FROM GetUserRoles()")
        //        .ToListAsync();

        //    // Nhóm người dùng theo vai trò 
        //    var groupedUsers = users
        //        .GroupBy(u => u.VaiTro)  // Nhóm theo vai trò
        //        .Select(group => new
        //        {
        //            VaiTro = group.Key,  // giá trị vai trò của từng nhóm
        //            Users = group.ToList()  // Danh sách người dùng trong mỗi vai trò
        //        })
        //        .ToList();

        //    // Trả về kết quả nhóm
        //    return Ok(groupedUsers);
        //}
    }

}

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApi_HienLTH.Data;
using WebApi_HienLTH.Models;
using WebApi_HienLTH.Models.DTO;
using WebApi_HienLTH.Repository.Repository;

namespace WebApi_HienLTH.Repository.NguoiDungRepository
{
    public class NguoiDungRepository : RepositoryBase, INguoiDungRepository
    {
        //kế thừa RepositoryBase thì sẽ kế thừa luôn 2 thuộc tính context và mapper
        public NguoiDungRepository(MyDbContext context, IMapper mapper) : base(context, mapper) { }

        public async Task CreateUser(NguoiDungModel model)
        {
            var user = _mapper.Map<NguoiDungEntity>(model);
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
        {
            //không dùng automapper
            //var roles = await _context.Roles.Select(r => new RoleDto
            //{
            //    Id= r.Id,
            //    RoleName=r.RoleName
            //}).ToListAsync();

            var roles = await _context.Roles.ToListAsync();
            var r = _mapper.Map<IEnumerable<RoleDto>>(roles);
            return r;
        }



        public async Task<IEnumerable<UserWithRolesDto>> GetAllUserWithRolesAsync()
        {
            var users = await _context.NguoiDungs
                        .Include(u => u.UserRoles)
                        .ThenInclude(ur => ur.Role)
                        .Select(u => new UserWithRolesDto
                        {
                            Id = u.Id,
                            UserName = u.UserName,
                            HoTen = u.HoTen,
                            Email = u.Email,
                            VaiTro = u.UserRoles.Any() ?
                                    u.UserRoles.Select(ur => ur.Role.RoleName).ToList() :
                                    new List<string> { "Không có vai trò" }
                        }).ToListAsync();
            return users;
        }

        public async Task<IEnumerable<object>> GetUsersGroupedByRoleAsync()
        {
            //lấy ra 
            var userWithRoles = await _context.NguoiDungs
                        .Include(nd => nd.UserRoles)
                        .ThenInclude(ur => ur.Role)
                        .ToListAsync();

            var groupedUserByRoles = userWithRoles
                        //selectmany làm phẳng tập data lồng nhau thành 1 tập 
                        //nếu ko selectmany thì trường hợp 1 phần tử có nhiều vai trò sẽ không nhóm được
                        .SelectMany(u => u.UserRoles.Select(ur => new
                        {
                            RoleName = ur.Role.RoleName,
                            User = u
                        }))
                        .GroupBy(x => x.RoleName) //nhóm theo rolename, x là kiểu vừa tạo ở trên
                        .Select(group => new
                        {
                            Role = group.Key,
                            Users = group.Select(x => new
                            {
                                x.User.Id,
                                x.User.UserName,
                                x.User.HoTen,
                                x.User.Email
                            }).ToList()
                        }).ToList();

            return groupedUserByRoles;
        }
    }
}

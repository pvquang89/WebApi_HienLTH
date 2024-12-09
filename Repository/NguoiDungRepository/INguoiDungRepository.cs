using WebApi_HienLTH.Data;
using WebApi_HienLTH.Models;
using WebApi_HienLTH.Models.DTO;

namespace WebApi_HienLTH.Repository.NguoiDungRepository
{
    public interface INguoiDungRepository
    {
        Task<IEnumerable<UserWithRolesDto>> GetAllUserWithRolesAsync();
        Task<IEnumerable<RoleDto>> GetAllRolesAsync();
        Task CreateUser(NguoiDungModel model);
        Task<IEnumerable<object>> GetUsersGroupedByRoleAsync();

    }
}

using WebApi_HienLTH.Data;

namespace WebApi_HienLTH.Repository.NguoiDungRepository
{
    public interface IAuthRepository
    {
        NguoiDungEntity Authenticate (string username, string password);
    }
}

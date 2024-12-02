using WebApi_HienLTH.Data;

namespace WebApi_HienLTH.Repository.NguoiDungRepository
{
    public interface INguoiDungRepository
    {
        NguoiDungEntity Authenticate (string username, string password);
    }
}

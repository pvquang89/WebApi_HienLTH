using AutoMapper;
using WebApi_HienLTH.Data;
using WebApi_HienLTH.Repository.Repository;

namespace WebApi_HienLTH.Repository.NguoiDungRepository
{
    public class NguoiDungRepository : RepositoryBase, INguoiDungRepository
    {
        public NguoiDungRepository(MyDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public NguoiDungEntity Authenticate(string username, string password)
        {
            return _context.NguoiDungs.SingleOrDefault(e => e.UserName == username && e.Password == password);
        }
    }
}

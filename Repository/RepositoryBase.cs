using AutoMapper;
using WebApi_HienLTH.Data;

namespace WebApi_HienLTH.Repository
{
    //class này để các repository kế thừa, ko phải tạo các field lặp lại ở mỗi repository
    public class RepositoryBase
    {
        protected readonly MyDbContext _context;
        protected readonly IMapper _mapper;
        public RepositoryBase(MyDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
    }
}

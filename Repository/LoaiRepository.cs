using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MyWebApiApp.Data;
using WebApi_HienLTH.Data;
using WebApi_HienLTH.Models;

namespace WebApi_HienLTH.Repository
{
    public class LoaiRepository : IGenericRepository<LoaiModel>
    {
        private readonly MyDbContext _context;
        private readonly IMapper _mapper;

        public LoaiRepository(MyDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LoaiModel>> GetAllAsync()
        {
            var entities = await _context.Loais.ToListAsync();
            return _mapper.Map<IEnumerable<LoaiModel>>(entities);
        }

        //thêm ? sau LoaiEntity cho phép nó có thể null vì FindAsync() có thể trả về null
        public async Task<LoaiModel?> GetByIdAsync(int id)
        {
            var entity = await _context.Loais.FindAsync(id);
            return _mapper.Map<LoaiModel>(entity);
        }

        public async Task CreateAsync(LoaiModel model)
        {
            // Map từ LoaiModel sang LoaiEntity
            //vì model là để tương tác với client còn chuyển sang entity mới tương tác vưới database
            var entity = _mapper.Map<LoaiEntity>(model);
            await _context.Loais.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Loais.FindAsync(id);
            if (entity != null)
            {
                _context.Loais.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(LoaiModel model)
        {
            var entity = _mapper.Map<LoaiEntity>(model);
            _context.Loais.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Loais.AnyAsync(e => e.MaLoai == id);
        }


    }
}

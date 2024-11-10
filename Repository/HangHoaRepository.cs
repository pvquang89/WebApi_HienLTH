using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApi_HienLTH.Data;
using WebApi_HienLTH.Models;

namespace WebApi_HienLTH.Repository
{
    public class HangHoaRepository : RepositoryBase, IGenericRepository<HangHoaModel>
    {
        public HangHoaRepository(MyDbContext context, IMapper mapper) : base(context, mapper)
        {
        }
        public async Task CreateAsync(HangHoaModel model)
        {
            //map từ model gửi từ client sang entity để thao tác với database
            var entity = _mapper.Map<HangHoaEntity>(model);
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.HangHoas.FindAsync(id);
            if (entity != null)
            {
                _context.HangHoas.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.HangHoas.AnyAsync(e => e.MaHh == id);
        }

        public async Task<IEnumerable<HangHoaModel>> GetAllAsync()
        {
            var entities = await _context.HangHoas.Include(e => e.Loai).ToListAsync();
            //map từ hanghoa entity sang hanghoa model
            return _mapper.Map<IEnumerable<HangHoaModel>>(entities);
        }

        public async Task<HangHoaModel> GetByIdAsync(int id)
        {
            var entity = await _context.HangHoas.Include(e => e.Loai).FirstOrDefaultAsync(e => e.MaLoai == id);
            return _mapper.Map<HangHoaModel>(entity);
        }

        public async Task UpdateAsync(HangHoaModel model)
        {
            //model được gửi từ client nên phải chuyển sang entity mới update được
            var entity = _mapper.Map<HangHoaEntity>(model);
            _context.HangHoas.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}

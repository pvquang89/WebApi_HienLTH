using Microsoft.EntityFrameworkCore;
using MyWebApiApp.Data;
using WebApi_HienLTH.Data;

namespace WebApi_HienLTH.Repository
{
    public class LoaiRepository : IGenericRepository<LoaiEntity>
    {
        public MyDbContext _context;

        public LoaiRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LoaiEntity>> GetAllAsync() => await _context.Loais.ToListAsync();


        //thêm ? sau LoaiEntity cho phép nó có thể null vì FindAsync() có thể traer về null
        public async Task<LoaiEntity?> GetByIdAsync(Guid id) => await _context.Loais.FindAsync(id);

        public async Task CreateAsync(LoaiEntity entity)
        {
            await _context.Loais.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.Loais.FindAsync(id);
            if (entity != null)
            {
                _context.Loais.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(LoaiEntity entity)
        {
            _context.Loais.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}

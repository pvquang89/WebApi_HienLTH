using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApi_HienLTH.Data;
using WebApi_HienLTH.Models;

namespace WebApi_HienLTH.Repository
{
    public class DonHangChiTietRepository : RepositoryBase, IGenericRepository<DonHangChiTietModel>
    {
        public DonHangChiTietRepository(MyDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task CreateAsync(DonHangChiTietModel model)
        {
            var entity = _mapper.Map<DonHangChiTietEntity>(model);
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        //ở đây dù kiểu trả về là DonHangChiTietModel nhưng vẫn return DonHangChiTietModelExtend nhờ tính đa hình OOP
        //nếu đổi kiểu trả về thành DonHangChiTietModelExtend sẽ sai vì tính kế thừa ko áp dụng cho danh sách
        public async Task<IEnumerable<DonHangChiTietModel>> GetAllAsync()
        {
            var entities = await _context.DonHangChiTiets.Include(e => e.HangHoa)
                                .OrderBy(e => e.MaDh).ToListAsync();
            return _mapper.Map<IEnumerable<DonHangChiTietModelExtend>>(entities);
        }
        //nếu đổi kiểu trả về thành DonHangChiTietModelExtend vẫn đúng vì nó là 1 đối tượng đơn lẻ và kế thừa DonHangChiTietModel 
        public async Task<DonHangChiTietModelExtend> GetByIdAsync(int maDh, int maHh)
        {
            var entity = await _context.DonHangChiTiets.Include(e => e.HangHoa).
                FirstOrDefaultAsync(e => e.MaDh == maDh && e.MaHh == maHh);
            return _mapper.Map<DonHangChiTietModelExtend>(entity);
        }
        public async Task DeleteAsync(int maDh, int maHh)
        {
            var entity = await _context.DonHangChiTiets.
                FirstOrDefaultAsync(e => e.MaDh == maDh && e.MaHh == maHh);
            if (entity != null)
            {
                _context.DonHangChiTiets.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
        public async Task UpdateAsync(DonHangChiTietModel model)
        {
            if (!await ExistsAsync(model.MaDh, model.MaHh))
                throw new KeyNotFoundException("Chi tiết đơn hàng không tồn tại.");

            //cách này tạo 1 obj mới từ src
            //var entity = _mapper.Map<DonHangChiTietEntity>(model);
            //_context.DonHangChiTiets.Update(entity);

            var entity = await _context.DonHangChiTiets
                .FirstOrDefaultAsync(e => e.MaDh == model.MaDh && e.MaHh == model.MaHh);
            if (entity != null)
            {
                //cách này cập nhật dữ liệu từ src sang dst nên ko cần_context.update()
                //_mapper.Map(model, entity);
                entity.SoLuong = model.SoLuong;
                entity.DonGia = model.DonGia;
                entity.GiamGia = model.GiamGia;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int maDh, int maHh)
        {
            return await _context.DonHangChiTiets.
                 AnyAsync(e => e.MaDh == maDh && e.MaHh == maHh);
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
        public Task<bool> ExistsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<DonHangChiTietModel> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }


    }
}

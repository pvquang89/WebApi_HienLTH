using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApi_HienLTH.Data;
using WebApi_HienLTH.Models;
using WebApi_HienLTH.Repository.Repository;

namespace WebApi_HienLTH.Repository
{
    public class DonHangChiTietRepository : RepositoryBase, IGenericRepository<DonHangChiTietModel>
    {
        public static int PAGE_SIZE { get; set; } = 5;

        public DonHangChiTietRepository(MyDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        //ở đây dù kiểu trả về là DonHangChiTietModel nhưng vẫn return DonHangChiTietModelExtend nhờ tính đa hình OOP
        //nếu đổi kiểu trả về thành DonHangChiTietModelExtend sẽ sai vì tính kế thừa ko áp dụng cho danh sách
        public async Task<IEnumerable<DonHangChiTietModel>> GetAllAsync(string? tenHh, int? maDh, string? sortBy, int page=1)
        {
            var query = _context.DonHangChiTiets.Include(e => e.HangHoa).AsQueryable();

            #region Tìm theo tên, mã đơn hàng
            if (!string.IsNullOrEmpty(tenHh))
            {
                query = query.Where(e => EF.Functions.Like(e.HangHoa.TenHh, $"%{tenHh}%"));
            }
            //HasValue : true - nếu maDh có giá trị, false nếu null
            if (maDh.HasValue)
            {
                query = query.Where(e => e.MaDh == maDh.Value);
            }
            #endregion

            #region sắp xếp
            query = sortBy?.ToLower() switch
            {
                "tenhanghoa" => query.OrderBy(e => e.HangHoa.TenHh),
                "soluong" => query.OrderBy(e => e.SoLuong),
                "soluonggiamdan" => query.OrderByDescending(e => e.SoLuong),
                _ => query.OrderBy(e => e.MaDh) // Mặc định sắp xếp theo mã đơn hàng
            };

            #endregion

            #region paging
            query = query.Skip((page - 1) * PAGE_SIZE).Take(PAGE_SIZE);
            #endregion
            var entities = await query.ToListAsync();
            return _mapper.Map<IEnumerable<DonHangChiTietModelExtend>>(entities);
        }
        public async Task CreateAsync(DonHangChiTietModel model)
        {
            var entity = _mapper.Map<DonHangChiTietEntity>(model);
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
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

        //1 số chức năng khác

        public async Task<double> GetTotalValueByMaDh(int maDh)
        {
            var totalValue = await _context.DonHangChiTiets.Where(e => e.MaDh == maDh)
                                    .SumAsync(e => e.SoLuong * e.DonGia); //ở mỗi phần tử sẽ tính như này và cuối cùng sẽ tổng lại
            return totalValue;
        }

        //các hàm dưới đã được override 

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.DonHangChiTiets.AnyAsync(e => e.MaDh == id);
        }

        public Task<DonHangChiTietModel> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DonHangChiTietModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
    }
}

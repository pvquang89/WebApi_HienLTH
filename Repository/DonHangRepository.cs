using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApi_HienLTH.Data;
using WebApi_HienLTH.Models;

namespace WebApi_HienLTH.Repository
{
    public class DonHangRepository : RepositoryBase, IGenericRepository<DonHangModel>
    {
        public DonHangRepository(MyDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task CreateAsync(DonHangModel model)
        {
            var entity = _mapper.Map<DonHangEntity>(model);
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.DonHangs.FindAsync(id);
            if (entity != null)
            {
                _context.DonHangs.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.DonHangs.AnyAsync(e => e.MaDh == id);
        }

        //Ở đây, kiểu trả về là DonHangModle nhưng thực sự trả về là DonHangDetailModel
        //vẫn được vì nó kế thừa lại DonHangModel => Tính đa hình
        public async Task<IEnumerable<DonHangModel>> GetAllAsync()
        {
            //throw new NotImplementedException();
            var entities = await _context.DonHangs
                          .Select(dh => new DonHangDetailModel
                          {
                              MaDh = dh.MaDh,
                              NguoiNhan = dh.NguoiNhan,
                              DiaChiGiao = dh.DiaChiGiao,
                              SoDienThoai = dh.SoDienThoai,
                              NgatDat = dh.NgatDat,
                              NgayGiao = dh.NgayGiao,
                              TinhTrangDonHang = dh.TinhTrangDonHang.ToString(),
                              TongGiaTri = dh.DonHangChiTiets.Sum(dhct => dhct.SoLuong * dhct.DonGia)
                          })
                          .OrderByDescending(dh => dh.TongGiaTri)
                          .ToListAsync();

            return entities;
        }

        public async Task<DonHangModel> GetByIdAsync(int id)
        {
            var entity = await _context.DonHangs
                       .Where(dh => dh.MaDh == id)
                       .Select(dh => new DonHangDetailModel
                       {
                           MaDh = dh.MaDh,
                           NguoiNhan = dh.NguoiNhan,
                           DiaChiGiao = dh.DiaChiGiao,
                           SoDienThoai = dh.SoDienThoai,
                           NgatDat = dh.NgatDat,
                           NgayGiao = dh.NgayGiao,
                           TinhTrangDonHang = dh.TinhTrangDonHang.ToString(),
                           TongGiaTri = dh.DonHangChiTiets.Sum(dhct => dhct.SoLuong * dhct.DonGia)
                       })
                       .FirstOrDefaultAsync();
            return entity;
        }

        public async Task UpdateAsync(DonHangModel model)
        {
            var entity = _mapper.Map<DonHangEntity>(model);
            _context.DonHangs.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}

using WebApi_HienLTH.Models;
using WebApi_HienLTH.Repository;
using WebApi_HienLTH.Repository.NguoiDungRepository;
using WebApi_HienLTH.Repository.Repository;

namespace WebApi_HienLTH.UnitOfWork
{
    //IDisposable : giải phóng tài nguyên khi ko quản lý
    public interface IUnitOfWork : IDisposable
    {
        //các repository implement IGenericRepository
        IGenericRepository<DonHangModel> DonHangRepository { get; }
        IGenericRepository<HangHoaModel> HangHoaRepository { get; }
        IGenericRepository<LoaiModel> LoaiRepository { get; }

        //các repository cụ thể 
        DonHangChiTietRepository DonHangChiTietRepository { get; } 
        IAuthRepository AuthRepository { get; }
        INguoiDungRepository NguoiDungRepository { get; }
        //
        Task SaveAsync();
    }
}

using AutoMapper;
using WebApi_HienLTH.Data;
using WebApi_HienLTH.Helper;
using WebApi_HienLTH.Models;
using WebApi_HienLTH.Repository;
using WebApi_HienLTH.Repository.NguoiDungRepository;
using WebApi_HienLTH.Repository.Repository;

namespace WebApi_HienLTH.UnitOfWork
{
    //primary constructor
    public class UnitOfWork(MyDbContext context, IMapper mapper) : IUnitOfWork
    {
        private IGenericRepository<DonHangModel> _donHangRepository;
        private IGenericRepository<HangHoaModel> _hangHoaRepository;
        private IGenericRepository<LoaiModel> _loaiRepository;
        private DonHangChiTietRepository _donHangChiTietRepository;
        private IAuthRepository _authRepository;
        private INguoiDungRepository _nguoiDungRepository;

        public IGenericRepository<DonHangModel> DonHangRepository
            => _donHangRepository ??= new DonHangRepository(context, mapper);

        public IGenericRepository<HangHoaModel> HangHoaRepository
            => _hangHoaRepository ??= new HangHoaRepository(context, mapper);

        public IGenericRepository<LoaiModel> LoaiRepository
            => _loaiRepository ??= new LoaiRepository(context, mapper);

        public DonHangChiTietRepository DonHangChiTietRepository
            => _donHangChiTietRepository ??= new DonHangChiTietRepository(context, mapper);

        public IAuthRepository AuthRepository
            => _authRepository ??= new AuthRepository(context, mapper);

        public INguoiDungRepository NguoiDungRepository
            => _nguoiDungRepository ??= new NguoiDungRepository(context, mapper);

        public void Dispose() => context.Dispose();

        public async Task SaveAsync() => await context.SaveChangesAsync();

    }
}

using AutoMapper;
using MyWebApiApp.Data;
using WebApi_HienLTH.Data;
using WebApi_HienLTH.Models;

namespace WebApi_HienLTH.Helper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            //Map 2 chiều từ LoaiEntity sang LoaiModel và ngược lại
            CreateMap<LoaiEntity, LoaiModel>().ReverseMap();

            //ForMember : ánh xạ cho thuộc tính
            CreateMap<HangHoaEntity, HangHoaModel>()
                //đoạn này : ánh xạ TenLoai từ Loai trong src vào TenLoai trong dest
                //cách đọc tương tự CreateMap(), map từ tham số thứ 2 sang tham số thứ 1
                .ForMember(dest => dest.TenLoai, // thuộc tính đích muốn map đến
                            opt => opt.MapFrom(src => src.Loai.TenLoai)) //map từ src vào dest
                .ReverseMap();

            CreateMap<DonHangEntity, DonHangModel>().ReverseMap();

            //cấu hình mapping cho DonHangChiTiet
            CreateMap<DonHangChiTietEntity, DonHangChiTietModelExtend>()
                .ForMember(dest => dest.TenHangHoa,
                            opt => opt.MapFrom(src => src.HangHoa.TenHh));

            CreateMap<DonHangChiTietEntity, DonHangChiTietModel>()
                .ReverseMap();

        }

    }
}

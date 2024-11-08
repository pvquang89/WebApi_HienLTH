using AutoMapper;
using MyWebApiApp.Data;
using WebApi_HienLTH.Models;

namespace WebApi_HienLTH.Helper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            //Map 2 chiều từ LoaiEntity sang LoaiModel và ngược lại
            CreateMap<LoaiEntity, LoaiModel>().ReverseMap();
        }

    }
}

using Microsoft.AspNetCore.Mvc;
using WebApi_HienLTH.Models;
using WebApi_HienLTH.Repository;

namespace WebApi_HienLTH.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoaiController : ControllerBase
    {

        private readonly IGenericRepository<LoaiModel> _loaiRepository;

        public LoaiController(IGenericRepository<LoaiModel> loaiRepository)
        {
            _loaiRepository = loaiRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoaiModel>>> GetAll()
        {
            var loais = await _loaiRepository.GetAllAsync();
            return Ok(loais);
        }
        

    }
}

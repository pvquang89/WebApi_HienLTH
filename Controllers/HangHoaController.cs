using Microsoft.AspNetCore.Mvc;
using WebApi_HienLTH.Models;
using WebApi_HienLTH.Repository;

namespace WebApi_HienLTH.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HangHoaController : ControllerBase
    {
        private readonly IGenericRepository<HangHoaModel> _hangHoaRepository;

        public HangHoaController(IGenericRepository<HangHoaModel> hangHoaRepository)
        {
            _hangHoaRepository = hangHoaRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<HangHoaModel>>> GetAll()
        {
            var entity = await _hangHoaRepository.GetAllAsync();
            return Ok(entity);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<HangHoaModel>> GetById(int id)
        {
            var entity = await _hangHoaRepository.GetByIdAsync(id);
            return entity == null ? NotFound("Không có hàng hoá chứa id này") : Ok(entity);
        }

        [HttpPost]
        public async Task<ActionResult<LoaiModel>> Create([FromBody] HangHoaModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _hangHoaRepository.CreateAsync(model);
            return CreatedAtAction(nameof(GetById), new { id = model.MaHh }, model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] HangHoaModel model)
        {
            if (id != model.MaHh)
                return BadRequest("Id không trùng khớp");
            if (!await _hangHoaRepository.ExistsAsync(id))
                return NotFound("Không tồn tại hàng hoá có id này");

            await _hangHoaRepository.UpdateAsync(model);
            return Ok("Cập nhật thành công");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _hangHoaRepository.ExistsAsync(id))
                return NotFound("Không thấy hàng hoá với id này");
            await _hangHoaRepository.DeleteAsync(id);
            return Ok("Xoá thành công");
        }
    }
}

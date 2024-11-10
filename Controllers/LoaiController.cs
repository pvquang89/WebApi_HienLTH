using Microsoft.AspNetCore.Mvc;
using WebApi_HienLTH.Models;
using WebApi_HienLTH.Repository;

namespace WebApi_HienLTH.Controllers
{
    [Route("api/[controller]/[action]")]
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var loai = await _loaiRepository.GetByIdAsync(id);
            if (loai == null)
                return NotFound("Loại này không tồn tại");
            return Ok(loai);
        }

        [HttpPost]
        public async Task<ActionResult<LoaiModel>> Create(LoaiModel model)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            await _loaiRepository.CreateAsync(model);
            return CreatedAtAction(nameof(GetById), new { id = model.MaLoai }, model);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] LoaiModel model)
        {
            if (id != model.MaLoai)
                return BadRequest("ID không trùng khớp.");

            await _loaiRepository.UpdateAsync(model);
            return Ok("Cập nhật thành công");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            if(!await _loaiRepository.ExistsAsync(id))
            {
                return NotFound("Loại này không tồn tại");
            }
            await _loaiRepository.DeleteAsync(id);
            return Ok("Xoá thành công");
        }

    }
}

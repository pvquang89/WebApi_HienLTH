using Microsoft.AspNetCore.Mvc;
using WebApi_HienLTH.Models;
using WebApi_HienLTH.Repository.Repository;

namespace WebApi_HienLTH.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DonHangController : ControllerBase
    {
        private readonly IGenericRepository<DonHangModel> _donHangRepository;

        public DonHangController(IGenericRepository<DonHangModel> donHangRepository)
        {
            _donHangRepository = donHangRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DonHangModel>>> GetAll()
        {
            var entities = await _donHangRepository.GetAllAsync();
            return Ok(entities);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DonHangModel>> GetById(int id)
        {
            var entity = await _donHangRepository.GetByIdAsync(id);
            return entity == null ? NotFound("Không tìm thấy") : Ok(entity);
        }

        [HttpPost]
        public async Task<ActionResult<DonHangModel>> Create(DonHangModel model)
        {
            await _donHangRepository.CreateAsync(model);
            return CreatedAtAction(nameof(GetById), new { id = model.MaDh }, model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, DonHangModel model)
        {
            if (id != model.MaDh)
                return BadRequest("Mã đơn hàng không khớp");
            if (!await _donHangRepository.ExistsAsync(id))
                return NotFound("Không tồn tại đơn hàng có id này");

            await _donHangRepository.UpdateAsync(model);
            return Ok("Cập nhật thành công");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _donHangRepository.ExistsAsync(id))
                return NotFound("Không thấy đơn hàng với id này");
            await _donHangRepository.DeleteAsync(id);
            return Ok("Xoá thành công");
        }
    }
}

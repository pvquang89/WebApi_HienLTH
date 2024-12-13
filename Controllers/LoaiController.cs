using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi_HienLTH.Models;
using WebApi_HienLTH.Repository.Repository;
using WebApi_HienLTH.UnitOfWork;

namespace WebApi_HienLTH.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoaiController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public LoaiController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoaiModel>>> GetAll()
        {
            var loais = await _unitOfWork.LoaiRepository.GetAllAsync();
            return Ok(loais);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var loai = await _unitOfWork.LoaiRepository.GetByIdAsync(id);
            if (loai == null)
                return NotFound("Loại này không tồn tại");
            return Ok(loai);
        }

        [HttpPost]
        //thêm [Authorize] để test JWT, nếu chưa đăng nhập sẽ ko được thêm mới
        public async Task<ActionResult<LoaiModel>> Create(LoaiModel model)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            await _unitOfWork.LoaiRepository.CreateAsync(model);
            return CreatedAtAction(nameof(GetById), new { id = model.MaLoai }, model);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] LoaiModel model)
        {
            if (id != model.MaLoai)
                return BadRequest("ID không trùng khớp.");

            await _unitOfWork.LoaiRepository.UpdateAsync(model);
            return Ok("Cập nhật thành công");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            if(!await _unitOfWork.LoaiRepository.ExistsAsync(id))
            {
                return NotFound("Loại này không tồn tại");
            }
            await _unitOfWork.LoaiRepository.DeleteAsync(id);
            return Ok("Xoá thành công");
        }

    }
}

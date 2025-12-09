using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PickGo_backend;
using PickGo_backend.DTOs.Request;
using PickGo_backend.Models;

namespace PickGo_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RequestController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RequestController(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // POST: api/Request
        [HttpPost]
        public async Task<IActionResult> Create(RequestCreateDTO dto)
        {
            var request = _mapper.Map<Request>(dto);
            request.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.RequestRepo.AddAsync(request);
            await _unitOfWork.SaveAsync();

            var result = _mapper.Map<RequestReadDTO>(request);
            return Ok(result);
        }

        // GET: api/Request
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var requests = await _unitOfWork.RequestRepo.GetAllAsync();
            var result = _mapper.Map<IEnumerable<RequestReadDTO>>(requests);
            return Ok(result);
        }

        // GET: api/Request/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var request = await _unitOfWork.RequestRepo.GetWithPackagesAsync(id);
            if (request == null) return NotFound();

            var result = _mapper.Map<RequestReadDTO>(request);
            return Ok(result);
        }

        // PUT: api/Request/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, RequestUpdateDTO dto)
        {
            var request = await _unitOfWork.RequestRepo.GetByIdAsync(id);
            if (request == null) return NotFound();

            // Manual partial update to avoid null overwrites
            if (dto.Source != null) request.Source = dto.Source;
            if (dto.PickupLat.HasValue) request.PickupLat = dto.PickupLat.Value;
            if (dto.PickupLng.HasValue) request.PickupLng = dto.PickupLng.Value;
            if (dto.SupplierId.HasValue) request.SupplierId = dto.SupplierId.Value;
            if (dto.Status.HasValue) request.Status = dto.Status.Value;

            _unitOfWork.RequestRepo.Update(request);
            await _unitOfWork.SaveAsync();

            var result = _mapper.Map<RequestReadDTO>(request);
            return Ok(result);
        }

        // DELETE: api/Request/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var request = await _unitOfWork.RequestRepo.GetByIdAsync(id);
            if (request == null) return NotFound();

            _unitOfWork.RequestRepo.Delete(request);
            await _unitOfWork.SaveAsync();

            return Ok(new { message = "Request deleted successfully" });
        }
    }
}

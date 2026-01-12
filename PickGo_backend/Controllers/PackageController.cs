using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PickGo_backend.DTOs.Package;
using PickGo_backend.Models;

namespace PickGo_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PackageController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PackageController(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Create(PackageCreateDTO dto)
        {
            var request = await _unitOfWork.RequestRepo.GetActiveRequestForCustomerAsync(dto.CustomerID ?? 0);
            if (request == null) return BadRequest("No active request found for this customer.");

            var package = _mapper.Map<Package>(dto);
            package.RequestID = request.Id;
            package.Status = PackageStatus.Pending;

            await _unitOfWork.PackageRepo.AddAsync(package);
            await _unitOfWork.SaveAsync();

            var result = _mapper.Map<PackageReadDTO>(package);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var pkg = await _unitOfWork.PackageRepo.GetByIdAsync(id);
            if (pkg == null) return NotFound();

            var result = _mapper.Map<PackageReadDTO>(pkg);
            return Ok(result);
        }

        [HttpGet("by-request/{requestId:int}")]
        public async Task<IActionResult> GetByRequest(int requestId)
        {
            var packages = await _unitOfWork.PackageRepo.GetByRequestIdAsync(requestId);
            var result = _mapper.Map<IEnumerable<PackageReadDTO>>(packages);
            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, PackageUpdateDTO dto)
        {
            var pkg = await _unitOfWork.PackageRepo.GetByIdAsync(id);
            if (pkg == null) return NotFound();

            _mapper.Map(dto, pkg);
            _unitOfWork.PackageRepo.Update(pkg);
            await _unitOfWork.SaveAsync();

            return Ok(_mapper.Map<PackageReadDTO>(pkg));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var pkg = await _unitOfWork.PackageRepo.GetByIdAsync(id);
            if (pkg == null) return NotFound();

            _unitOfWork.PackageRepo.Delete(pkg);
            await _unitOfWork.SaveAsync();

            return Ok(new { message = "Package deleted successfully" });
        }
    }
}

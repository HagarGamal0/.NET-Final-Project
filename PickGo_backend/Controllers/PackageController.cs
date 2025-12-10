using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PickGo_backend;
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

        // POST: api/Package
        [HttpPost]
        public async Task<IActionResult> Create(PackageCreateDTO dto)
        {
            // Optional: validate Request exists
            var request = await _unitOfWork.RequestRepo.GetByIdAsync(dto.RequestID);
            if (request == null) return BadRequest("RequestID is invalid.");

            var package = _mapper.Map<Package>(dto);

            await _unitOfWork.PackageRepo.AddAsync(package);
            await _unitOfWork.SaveAsync();

            var result = _mapper.Map<PackageReadDTO>(package);
            return Ok(result);
        }

        // GET: api/Package/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var pkg = await _unitOfWork.PackageRepo.GetByIdAsync(id);
            if (pkg == null) return NotFound();

            var result = _mapper.Map<PackageReadDTO>(pkg);
            return Ok(result);
        }

        // GET: api/Package/by-request/3
        [HttpGet("by-request/{requestId:int}")]
        public async Task<IActionResult> GetByRequest(int requestId)
        {
            var packages = await _unitOfWork.PackageRepo.GetByRequestIdAsync(requestId);
            var result = _mapper.Map<IEnumerable<PackageReadDTO>>(packages);
            return Ok(result);
        }

        // PUT: api/Package/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, PackageUpdateDTO dto)
        {
            var pkg = await _unitOfWork.PackageRepo.GetByIdAsync(id);
            if (pkg == null) return NotFound();

            if (dto.Description != null) pkg.Description = dto.Description;
            if (dto.Weight.HasValue) pkg.Weight = dto.Weight.Value;
            if (dto.Fragile.HasValue) pkg.Fragile = dto.Fragile.Value;
            if (dto.ExpireDate.HasValue) pkg.ExpireDate = dto.ExpireDate.Value;
            if (dto.ShipmentCost.HasValue) pkg.ShipmentCost = dto.ShipmentCost.Value;
            if (dto.Destination != null) pkg.Destination = dto.Destination;
            if (dto.Lat.HasValue) pkg.Lat = dto.Lat.Value;
            if (dto.Lang.HasValue) pkg.Lang = dto.Lang.Value;
            if (dto.Status.HasValue) pkg.Status = dto.Status.Value;
            if (dto.ShipmentNotes != null) pkg.ShipmentNotes = dto.ShipmentNotes;

            _unitOfWork.PackageRepo.Update(pkg);
            await _unitOfWork.SaveAsync();

            var result = _mapper.Map<PackageReadDTO>(pkg);
            return Ok(result);
        }

        // DELETE: api/Package/5
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

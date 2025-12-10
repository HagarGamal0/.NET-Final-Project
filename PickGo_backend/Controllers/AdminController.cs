using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PickGo_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]

    public class AdminController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public AdminController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // -------------------- Get all pending Couriers --------------------
        [HttpGet("PendingCouriers")]
        public async Task<IActionResult> GetPendingCouriers()
        {
            var roleClaims = User.Claims.Where(c => c.Type.Contains("role"));
            foreach (var claim in roleClaims)
                Console.WriteLine($"Claimmmmmmmmmmmmmmmmm: {claim.Type} = {claim.Value}");


            var pending = (await _unitOfWork.CourierRepo.GetAllAsync())
                          .Where(c => c.Status == "Pending").ToList();

            return Ok(pending);
        }

        // -------------------- Approve Courier --------------------
        [HttpPost("ApproveCourier/{courierId}")]
        public async Task<IActionResult> ApproveCourier(int courierId)
        {
            var courier = await _unitOfWork.CourierRepo.GetByIdAsync(courierId);
            if (courier == null) return NotFound("Courier not found.");

            courier.Status = "Approved";
            _unitOfWork.CourierRepo.Update(courier);
            await _unitOfWork.SaveAsync();

            return Ok(new { message = "Courier approved successfully." });
        }

        // -------------------- Reject Courier --------------------
        [HttpPost("RejectCourier/{courierId}")]
        public async Task<IActionResult> RejectCourier(int courierId)
        {
            var courier = await _unitOfWork.CourierRepo.GetByIdAsync(courierId);
            if (courier == null) return NotFound("Courier not found.");

            courier.Status = "Rejected";
            _unitOfWork.CourierRepo.Update(courier);
            await _unitOfWork.SaveAsync();

            return Ok(new { message = "Courier rejected successfully." });
        }
        // -------------------- Get Online Couriers --------------------
        [HttpGet("OnlineCouriers")]
        public async Task<IActionResult> GetOnlineCouriers()
        {
            var onlineCouriers = (await _unitOfWork.CourierRepo.GetAllAsync())
                                 .Where(c => c.IsOnline && c.Status == "Approved")
                                 .ToList();

            return Ok(onlineCouriers);
        }

    }
}


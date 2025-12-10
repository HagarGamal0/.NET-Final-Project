using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace PickGo_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CourierController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public CourierController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        // -------------------- Get Online Couriers --------------------
        [HttpGet("Online")]
        public async Task<IActionResult> GetOnlineCouriers()
        {
            var couriers = (await _unitOfWork.CourierRepo.GetAllAsync())
                           .Where(c =>
                               c.IsOnline &&
                               c.Status == "Approved"
                           )
                           .Select(c => new
                           {
                               c.Id,
                               c.UserId,
                               c.VehicleType,
                               c.Rating,
                               c.MaxWeight
                           })
                           .ToList();

            return Ok(couriers);
        }



        [HttpPost("ToggleOnlineStatus")]
        public async Task<IActionResult> ToggleOnlineStatus()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized();

            var courier = (await _unitOfWork.CourierRepo.GetAllAsync())
                          .FirstOrDefault(c => c.UserId == userId);

            if (courier == null)
                return NotFound("Courier profile not found.");
            
            if (courier.Status != "Approved")
                return BadRequest("Courier is not approved yet.");

            courier.IsOnline = !courier.IsOnline;

            _unitOfWork.CourierRepo.Update(courier);
            await _unitOfWork.SaveAsync();

            return Ok(new
            {
                message = courier.IsOnline
                    ? "You are now online."
                    : "You are now offline.",
                isOnline = courier.IsOnline
            });
        }
    }
}



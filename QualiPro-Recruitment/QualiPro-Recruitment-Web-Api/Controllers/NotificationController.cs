using Microsoft.AspNetCore.Mvc;
using QualiPro_Recruitment_Data.Models;
using QualiPro_Recruitment_Web_Api.Repositories.NotificationRepo;

namespace QualiPro_Recruitment_Web_Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationController(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        // POST: api/Notification
        [HttpPost]
        public async Task<ActionResult<Notification>> AddNotification([FromBody] Notification notification)
        {
            if (notification == null)
            {
                return BadRequest(new { message = "Notification data is required." });
            }

            var createdNotification = await _notificationRepository.AddNotificationAsync(notification);
            return CreatedAtAction(nameof(GetNotificationById), new { id = createdNotification.Id }, createdNotification);
        }

        // GET: api/Notification/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Notification>> GetNotificationById(int id)
        {
            var notification = await _notificationRepository.GetNotificationByIdAsync(id);

            if (notification == null)
            {
                return NotFound(new { message = "Notification not found." });
            }

            return Ok(notification);
        }

        // GET: api/Notification/User/{userId}
        [HttpGet("User/{userId}")]
        public async Task<ActionResult<IEnumerable<Notification>>> GetNotificationsByUserId(int userId)
        {
            var notifications = await _notificationRepository.GetNotificationsByUserIdAsync(userId);

            if (notifications == null || !notifications.Any())
            {
                return NotFound(new { message = "No notifications found for this user." });
            }

            return Ok(notifications);
        }

        // PUT: api/Notification/{id}/MarkAsRead
        [HttpPut("{id}/MarkAsRead")]
        public async Task<IActionResult> MarkNotificationAsRead(int id)
        {
            var notification = await _notificationRepository.GetNotificationByIdAsync(id);

            if (notification == null)
            {
                return NotFound(new { message = "Notification not found." });
            }

            notification.IsRead = true;
            await _notificationRepository.UpdateNotificationAsync(notification);

            return NoContent();
        }
    }
}

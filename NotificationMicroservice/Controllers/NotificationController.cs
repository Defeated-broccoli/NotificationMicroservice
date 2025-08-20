using Microsoft.AspNetCore.Mvc;
using NotificationMicroservice.Entities;
using NotificationMicroservice.Service;

namespace NotificationMicroservice.Controllers;

[ApiController]
[Route("[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly NotificationService _notificationService;

    public NotificationsController(NotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpPost]
    public async Task<IActionResult> Send([FromBody] NotificationDto dto)
    {
        if (dto == null)
        {
            return BadRequest("Notification data is required.");
        }

        var notification = new Notification(dto.Recipient, dto.Message, dto.Channel);
        var result = await _notificationService.SendAsync(notification);

        if (result == false)
        {
            return StatusCode(500, "Failed to send notification.");
        }

        return Ok("Notification sent (or queued).");
    }
}
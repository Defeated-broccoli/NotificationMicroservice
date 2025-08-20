using Microsoft.AspNetCore.Mvc;
using NotificationMicroservice.Entities;
using NotificationMicroservice.Interfaces;
using NotificationMicroservice.Service;
using NotificationMicroservice.Validators;

namespace NotificationMicroservice.Controllers;

[ApiController]
[Route("[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly NotificationService _notificationService;
    private readonly IValidator<NotificationDto> _notificationValidator;

    public NotificationsController(NotificationService notificationService, IValidator<NotificationDto> notificationValidator)
    {
        _notificationService = notificationService;
        _notificationValidator = notificationValidator;
    }

    [HttpPost]
    public async Task<IActionResult> Send([FromBody] NotificationDto dto)
    {
        var validationResult = _notificationValidator.Validate(dto);

        if (!validationResult.IsValid)
        {
            return BadRequest(new { message = validationResult.ErrorMessage });
        }

        var notification = new Notification(dto.Recipient, dto.Message, dto.Channel, dto.Sender);
        var result = await _notificationService.SendAsync(notification);

        if (result == false)
        {
            return StatusCode(503, new { message = "Failed to send notification." });
        }

        return Ok(new { message = "Notification queued for sending " });
    }
}
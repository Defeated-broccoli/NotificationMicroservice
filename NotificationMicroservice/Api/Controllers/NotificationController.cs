using Microsoft.AspNetCore.Mvc;
using NotificationMicroservice.Api.Dtos;
using NotificationMicroservice.Application.Services;
using NotificationMicroservice.Entities;

namespace NotificationMicroservice.Api.Controllers;

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
    public async Task<IActionResult> SendNotification([FromBody] NotificationDto dto)
    {
        var validationResult = Notification.TryCreate(dto);

        if (!validationResult.IsValid)
        {
            return BadRequest(new { message = validationResult.ErrorMessage });
        }

        var result = await _notificationService.SendAsync(validationResult.Value!);

        if (result == false)
        {
            return StatusCode(503, new { message = "Failed to send notification." });
        }

        return Ok(new { message = "Notification queued for sending " });
    }
}
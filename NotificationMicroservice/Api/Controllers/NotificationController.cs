using Microsoft.AspNetCore.Mvc;
using NotificationMicroservice.Api.Dtos;
using NotificationMicroservice.Application.Interfaces;
using NotificationMicroservice.Entities;
using NotificationMicroservice.Resources;

namespace NotificationMicroservice.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationsController(INotificationService notificationService)
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
            return StatusCode(503, new { message = NotificationMessages.FailedToSendNotification });
        }

        return Ok(new { message = NotificationMessages.NotificationQueued });
    }
}
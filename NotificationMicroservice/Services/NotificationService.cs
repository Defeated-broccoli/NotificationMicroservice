using NotificationMicroservice.Entities;
using NotificationMicroservice.Interfaces;

namespace NotificationMicroservice.Service;

public class NotificationService
{
    private readonly IQueueService _queueService;

    public NotificationService(IQueueService queueService)
    {
        _queueService = queueService;
    }

    public async Task<bool> SendAsync(Notification notification)
    {
        return await _queueService.EnqueueMessage(notification);
    }
}
using NotificationMicroservice.Entities;

namespace NotificationMicroservice.Application.Interfaces;

public interface IQueueService
{
    Task<bool> EnqueueNotification(Notification notification);
}
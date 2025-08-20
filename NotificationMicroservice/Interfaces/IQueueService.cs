using NotificationMicroservice.Entities;

namespace NotificationMicroservice.Interfaces;

public interface IQueueService
{
    Task<bool> EnqueueMessage(Notification notification);
}
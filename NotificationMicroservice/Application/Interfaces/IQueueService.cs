using NotificationMicroservice.Entities;

namespace NotificationMicroservice.Application.Interfaces;

public interface IQueueService
{
    Task<bool> EnqueueMessage(Notification notification);
}
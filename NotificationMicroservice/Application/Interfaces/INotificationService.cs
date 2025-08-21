using NotificationMicroservice.Entities;

namespace NotificationMicroservice.Application.Interfaces
{
    public interface INotificationService
    {
        Task<bool> SendAsync(Notification notification);
    }
}
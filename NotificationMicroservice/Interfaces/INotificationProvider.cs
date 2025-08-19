using NotificationMicroservice.Entities;
using NotificationMicroservice.Enums;

namespace NotificationMicroservice.Interfaces;

public interface INotificationProvider
{
    bool IsEnabled { get; }
    string Name { get; }
    int Priority { get; }
    ChannelType SupportedChannel { get; }

    Task<bool> SendAsync(Notification notification);
}
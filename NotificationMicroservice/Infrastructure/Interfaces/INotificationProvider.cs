using NotificationMicroservice.Domain.Enums;
using NotificationMicroservice.Entities;

namespace NotificationMicroservice.Infrastructure.Interfaces;

public interface INotificationProvider
{
    bool IsEnabled { get; }

    string Name { get; }

    int Priority { get; }

    ChannelType SupportedChannel { get; }

    Task<bool> SendAsync(Notification notification);
}
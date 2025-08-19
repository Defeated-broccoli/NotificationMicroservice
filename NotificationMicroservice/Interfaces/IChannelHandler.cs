using NotificationMicroservice.Entities;
using NotificationMicroservice.Enums;

namespace NotificationMicroservice.Interfaces
{
    public interface IChannelHandler
    {
        ChannelType SupportedChannel { get; }

        Task SendAsync(Notification notification);
    }
}
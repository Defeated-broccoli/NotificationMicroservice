using NotificationMicroservice.Entities;
using NotificationMicroservice.Enums;

namespace NotificationMicroservice.Interfaces
{
    public interface IChannelHandler
    {
        ChannelType SupportedChannel { get; }

        Task<bool> SendAsync(Notification notification);
    }
}
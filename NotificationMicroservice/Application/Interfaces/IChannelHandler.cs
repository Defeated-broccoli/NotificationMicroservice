using NotificationMicroservice.Domain.Enums;
using NotificationMicroservice.Entities;

namespace NotificationMicroservice.Application.Interfaces
{
    public interface IChannelHandler
    {
        ChannelType SupportedChannel { get; }

        Task<bool> SendAsync(Notification notification);
    }
}
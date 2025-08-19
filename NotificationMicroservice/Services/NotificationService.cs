using NotificationMicroservice.Entities;
using NotificationMicroservice.Enums;
using NotificationMicroservice.Interfaces;

namespace NotificationMicroservice.Service;

public class NotificationService
{
    private readonly Dictionary<ChannelType, IChannelHandler> _handlers;

    public NotificationService(IEnumerable<IChannelHandler> handlers)
    {
        _handlers = handlers.ToDictionary(h => h.SupportedChannel);
    }

    public async Task SendAsync(Notification notification)
    {
        if (_handlers.TryGetValue(notification.Channel, out var handler))
        {
            await handler.SendAsync(notification);
        }
        else
        {
            throw new NotSupportedException($"Channel {notification.Channel} not supported");
        }
    }
}
using NotificationMicroservice.Application.Interfaces;
using NotificationMicroservice.Domain.Enums;
using NotificationMicroservice.Entities;
using NotificationMicroservice.Infrastructure.Interfaces;

namespace NotificationMicroservice.Application.Handlers;

public abstract class BaseChannelHandler : IChannelHandler
{
    protected readonly IEnumerable<INotificationProvider> _providers;

    public abstract ChannelType SupportedChannel { get; }

    protected BaseChannelHandler(IEnumerable<INotificationProvider> providers)
    {
        _providers = providers
            .Where(p => p.SupportedChannel == SupportedChannel && p.IsEnabled)
            .OrderBy(p => p.Priority)
            .ToList();
    }

    public async Task<bool> SendAsync(Notification notification)
    {
        foreach (var provider in _providers)
        {
            var success = await provider.SendAsync(notification);
            if (success) return true;
        }

        return false;
    }
}
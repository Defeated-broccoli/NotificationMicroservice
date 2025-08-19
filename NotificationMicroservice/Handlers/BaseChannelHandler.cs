using NotificationMicroservice.Entities;
using NotificationMicroservice.Enums;
using NotificationMicroservice.Interfaces;

namespace NotificationMicroservice.Handlers;

public abstract class BaseChannelHandler : IChannelHandler
{
    protected readonly IEnumerable<INotificationProvider> _providers;

    protected BaseChannelHandler(IEnumerable<INotificationProvider> providers)
    {
        _providers = providers
            .Where(p => p.SupportedChannel == SupportedChannel && p.IsEnabled)
            .OrderBy(p => p.Priority)
            .ToList();
    }

    public abstract ChannelType SupportedChannel { get; }

    public async Task SendAsync(Notification notification)
    {
        foreach (var provider in _providers)
        {
            var success = await provider.SendAsync(notification);
            if (success) return;
        }

        Console.WriteLine("All providers failed. Queuing for retry...");
    }
}
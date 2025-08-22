using Microsoft.Extensions.Options;
using NotificationMicroservice.Domain.Enums;
using NotificationMicroservice.Entities;
using NotificationMicroservice.Infrastructure.Commons;
using NotificationMicroservice.Infrastructure.Interfaces;

namespace NotificationMicroservice.Infrastructure.Providers;

public class AmazonPushProvider : INotificationProvider
{
    private readonly ProviderConfig _providerConfig;

    public bool IsEnabled => _providerConfig.IsEnabled;

    public string? Name => _providerConfig.Name;

    public int Priority => _providerConfig.Priority;

    public ChannelType SupportedChannel => ChannelType.Push;

    public AmazonPushProvider(IOptionsSnapshot<Dictionary<string, ProviderConfig>> options)
    {
        _providerConfig = options.Value["AmazonPush"];
    }

    public async Task<bool> SendAsync(Notification notification)
    {
        try
        {
            Console.WriteLine($"Sending push notification");
            return true;
        }
        catch
        {
            return false;
        }
    }
}
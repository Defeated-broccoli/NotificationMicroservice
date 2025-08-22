using Microsoft.Extensions.Options;
using NotificationMicroservice.Domain.Enums;
using NotificationMicroservice.Entities;
using NotificationMicroservice.Infrastructure.Commons;
using NotificationMicroservice.Infrastructure.Interfaces;

namespace NotificationMicroservice.Infrastructure.Providers;

public class TwilioEmailProvider : INotificationProvider
{
    private readonly ProviderConfig _providerConfig;

    public bool IsEnabled => _providerConfig.IsEnabled;

    public string? Name => _providerConfig.Name;

    public int Priority => _providerConfig.Priority;

    public ChannelType SupportedChannel => ChannelType.Email;

    public TwilioEmailProvider(IOptionsSnapshot<Dictionary<string, ProviderConfig>> options)
    {
        _providerConfig = options.Value["TwilioEmail"];
    }

    public async Task<bool> SendAsync(Notification notification)
    {
        try
        {
            Console.WriteLine($"Sending email notification to {notification.ToEmailAddress}");
            return true;
        }
        catch
        {
            return false;
        }
    }
}
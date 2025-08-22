using Microsoft.Extensions.Options;
using NotificationMicroservice.Domain.Enums;
using NotificationMicroservice.Entities;
using NotificationMicroservice.Infrastructure.Commons;
using NotificationMicroservice.Infrastructure.Interfaces;

namespace NotificationMicroservice.Infrastructure.Providers;

public class AmazonSmsProvider : INotificationProvider
{
    private readonly ProviderConfig _providerConfig;

    public bool IsEnabled => _providerConfig.IsEnabled;

    public string? Name => _providerConfig.Name;

    public int Priority => _providerConfig.Priority;

    public ChannelType SupportedChannel => ChannelType.Sms;

    public AmazonSmsProvider(IOptionsSnapshot<Dictionary<string, ProviderConfig>> options)
    {
        _providerConfig = options.Value["AmazonSms"];
    }

    public async Task<bool> SendAsync(Notification notification)
    {
        try
        {
            Console.WriteLine($"Sending SMS notification to {notification.ToPhoneNumber}");
            return true;
        }
        catch
        {
            return false;
        }
    }
}
using NotificationMicroservice.Domain.Enums;
using NotificationMicroservice.Entities;
using NotificationMicroservice.Infrastructure.Interfaces;

namespace NotificationMicroservice.Infrastructure.Providers;

public class TwilioSmsProvider : INotificationProvider
{
    public bool IsEnabled => true;

    public string Name => "TwilioSms";

    public int Priority => 2;

    public ChannelType SupportedChannel => ChannelType.Sms;

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
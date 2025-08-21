using NotificationMicroservice.Domain.Enums;
using NotificationMicroservice.Entities;
using NotificationMicroservice.Infrastructure.Interfaces;

namespace NotificationMicroservice.Infrastructure.Providers;

public class TwilioEmailProvider : INotificationProvider
{
    public bool IsEnabled => true;

    public string Name => "TwilioEmail";

    public int Priority => 2;

    public ChannelType SupportedChannel => ChannelType.Email;

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
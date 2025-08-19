using NotificationMicroservice.Entities;
using NotificationMicroservice.Enums;
using NotificationMicroservice.Interfaces;

namespace NotificationMicroservice.Providers;

public class TwilioPushProvider : INotificationProvider
{
    public bool IsEnabled => true;

    public string Name => "TwilioPush";

    public int Priority => 3;

    public ChannelType SupportedChannel => ChannelType.Push;

    public async Task<bool> SendAsync(Notification notification)
    {
        try
        {
            // Call Twilio SDK (skeleton)
            Console.WriteLine($"[Twilio] Sending SMS to {notification.Recipient}: {notification.Message}");
            return true;
        }
        catch
        {
            return false;
        }
    }
}
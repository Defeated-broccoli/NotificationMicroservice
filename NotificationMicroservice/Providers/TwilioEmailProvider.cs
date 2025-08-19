using NotificationMicroservice.Entities;
using NotificationMicroservice.Enums;
using NotificationMicroservice.Interfaces;

namespace NotificationMicroservice.Providers;

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
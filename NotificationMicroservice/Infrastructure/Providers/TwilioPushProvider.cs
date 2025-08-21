using NotificationMicroservice.Domain.Enums;
using NotificationMicroservice.Entities;
using NotificationMicroservice.Infrastructure.Interfaces;

namespace NotificationMicroservice.Infrastructure.Providers;

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
            Console.WriteLine($"Sending push notification");
            return true;
        }
        catch
        {
            return false;
        }
    }
}
using Amazon.SQS;
using NotificationMicroservice.Domain.Enums;
using NotificationMicroservice.Entities;
using NotificationMicroservice.Infrastructure.Interfaces;
using System.Text.Json.Serialization;

namespace NotificationMicroservice.Infrastructure.Providers;

public class AmazonSmsProvider : INotificationProvider
{
    public bool IsEnabled => true;

    public string Name => "AmazonSms";

    public int Priority => 1;

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
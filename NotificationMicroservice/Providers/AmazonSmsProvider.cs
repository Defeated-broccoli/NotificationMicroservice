using Amazon.SQS;
using NotificationMicroservice.Entities;
using NotificationMicroservice.Enums;
using NotificationMicroservice.Interfaces;
using System.Text.Json.Serialization;

namespace NotificationMicroservice.Providers;

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
            return true;
        }
        catch
        {
            return false;
        }
    }
}
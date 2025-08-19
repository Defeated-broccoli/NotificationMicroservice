using NotificationMicroservice.Entities;
using NotificationMicroservice.Enums;
using NotificationMicroservice.Interfaces;

namespace NotificationMicroservice.Handlers;

public class SmsChannelHandler : BaseChannelHandler
{
    public SmsChannelHandler(IEnumerable<INotificationProvider> providers) : base(providers)
    {
    }

    public override ChannelType SupportedChannel => ChannelType.Sms;
}
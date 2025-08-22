using NotificationMicroservice.Domain.Enums;
using NotificationMicroservice.Infrastructure.Interfaces;

namespace NotificationMicroservice.Application.Handlers;

public class SmsChannelHandler : BaseChannelHandler
{
    public override ChannelType SupportedChannel => ChannelType.Sms;

    public SmsChannelHandler(IEnumerable<INotificationProvider> providers) : base(providers)
    {
    }
}
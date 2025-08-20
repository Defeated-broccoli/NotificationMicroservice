using Amazon.SQS;
using NotificationMicroservice.Enums;
using NotificationMicroservice.Interfaces;

namespace NotificationMicroservice.Handlers;

public class PushChannelHandler : BaseChannelHandler
{
    public PushChannelHandler(IEnumerable<INotificationProvider> providers) : base(providers)
    {
    }

    public override ChannelType SupportedChannel => ChannelType.Push;
}
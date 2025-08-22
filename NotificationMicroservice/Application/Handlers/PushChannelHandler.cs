using NotificationMicroservice.Domain.Enums;
using NotificationMicroservice.Infrastructure.Interfaces;

namespace NotificationMicroservice.Application.Handlers;

public class PushChannelHandler : BaseChannelHandler
{
    public override ChannelType SupportedChannel => ChannelType.Push;

    public PushChannelHandler(IEnumerable<INotificationProvider> providers) : base(providers)
    {
    }
}
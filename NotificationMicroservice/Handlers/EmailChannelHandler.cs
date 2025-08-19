using NotificationMicroservice.Enums;
using NotificationMicroservice.Interfaces;

namespace NotificationMicroservice.Handlers;

public class EmailChannelHandler : BaseChannelHandler
{
    public EmailChannelHandler(IEnumerable<INotificationProvider> providers) : base(providers)
    {
    }

    public override ChannelType SupportedChannel => ChannelType.Email;
}
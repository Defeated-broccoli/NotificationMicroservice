using NotificationMicroservice.Domain.Enums;
using NotificationMicroservice.Infrastructure.Interfaces;

namespace NotificationMicroservice.Application.Handlers;

public class EmailChannelHandler : BaseChannelHandler
{
    public override ChannelType SupportedChannel => ChannelType.Email;

    public EmailChannelHandler(IEnumerable<INotificationProvider> providers) : base(providers)
    {
    }
}
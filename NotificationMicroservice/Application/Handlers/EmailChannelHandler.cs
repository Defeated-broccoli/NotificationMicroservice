using Amazon.SQS;
using NotificationMicroservice.Domain.Enums;
using NotificationMicroservice.Infrastructure.Interfaces;

namespace NotificationMicroservice.Application.Handlers;

public class EmailChannelHandler : BaseChannelHandler
{
    public EmailChannelHandler(IEnumerable<INotificationProvider> providers) : base(providers)
    {
    }

    public override ChannelType SupportedChannel => ChannelType.Email;
}
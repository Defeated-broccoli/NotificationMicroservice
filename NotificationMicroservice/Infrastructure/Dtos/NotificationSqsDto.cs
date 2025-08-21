using NotificationMicroservice.Abstractions;
using NotificationMicroservice.Domain.Enums;

namespace NotificationMicroservice.Infrastructure.Dtos;

public class NotificationSqsDto : INotificationDto
{
    public string Body { get; set; }

    public ChannelType Channel { get; set; }

    public string Id { get; set; }

    public string Recipient { get; set; }

    public string Sender { get; set; }

    public string? Subject { get; set; }
}
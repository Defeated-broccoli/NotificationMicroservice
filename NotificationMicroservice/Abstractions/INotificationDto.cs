using NotificationMicroservice.Domain.Enums;

namespace NotificationMicroservice.Abstractions;

public interface INotificationDto
{
    string Body { get; set; }

    ChannelType Channel { get; set; }

    string Recipient { get; set; }

    string Sender { get; set; }

    string? Subject { get; set; }
}
using NotificationMicroservice.Api.Dtos;
using NotificationMicroservice.Entities;
using NotificationMicroservice.Infrastructure.Dtos;

namespace NotificationMicroservice.Infrastructure.Mappers;

public class NotificationMapper
{
    public static NotificationDto ToApiDto(Notification notification) =>
        new()
        {
            Channel = notification.Channel,
            Body = notification.Body,
            Recipient = notification.ToEmailAddress?.Value ?? notification.ToPhoneNumber?.Value,
            Sender = notification.FromEmailAddress?.Value ?? notification.FromPhoneNumber?.Value,
            Subject = notification.Subject
        };

    public static NotificationSqsDto ToSqsDto(Notification notification) =>
        new()
        {
            Id = notification.Id,
            Channel = notification.Channel,
            Body = notification.Body,
            Recipient = notification.ToEmailAddress?.Value ?? notification.ToPhoneNumber?.Value,
            Sender = notification.FromEmailAddress?.Value ?? notification.FromPhoneNumber?.Value,
            Subject = notification.Subject
        };
}
using NotificationMicroservice.Api.Dtos;
using NotificationMicroservice.Domain.Enums;
using NotificationMicroservice.Entities;
using System.Net.Mail;

namespace NotificationMicroserviceTests.TestCommons;

public class BaseTest
{
    protected static Notification CreateNotification()
    {
        var notificationDto = CreateNotificationDto();

        var notification = Notification.TryCreate(notificationDto);

        return notification.Value!;
    }

    protected static NotificationDto CreateNotificationDto(bool isValid = true, ChannelType channelType = ChannelType.Email)
    {
        var validEmail = "valid@email.com";
        var invalidEmail = "invalid@@@mail";
        var validPhone = "+1234567890";
        var invalidPhone = "123abc";

        string recipient;
        string sender;

        switch (channelType)
        {
            case ChannelType.Email:
                recipient = isValid ? validEmail : invalidEmail;
                sender = validEmail;
                break;

            case ChannelType.Sms:
                recipient = isValid ? validPhone : invalidPhone;
                sender = validPhone;
                break;

            case ChannelType.Push:
                recipient = "any";
                sender = "any";
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(channelType), channelType, "Unsupported channel type");
        }

        return new NotificationDto
        {
            Channel = channelType,
            Body = "This is a valid message body.",
            Recipient = recipient,
            Sender = sender,
            Subject = channelType == ChannelType.Email ? "Important email" : null
        };
    }
}
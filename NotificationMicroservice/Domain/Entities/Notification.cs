using NotificationMicroservice.Abstractions;
using NotificationMicroservice.Domain.Common;
using NotificationMicroservice.Domain.Enums;
using NotificationMicroservice.Domain.ValueObjects;
using NotificationMicroservice.Resources;

namespace NotificationMicroservice.Entities;

public sealed class Notification
{
    public MessageBody Body { get; }

    public ChannelType Channel { get; }

    public EmailAddress? FromEmailAddress { get; }

    public PhoneNumber? FromPhoneNumber { get; }

    public string Id { get; } = Guid.NewGuid().ToString();

    public string? Subject { get; }

    public EmailAddress? ToEmailAddress { get; }

    public PhoneNumber? ToPhoneNumber { get; }

    private Notification(
        ChannelType channel,
        MessageBody body,
        EmailAddress? fromEmailAddress,
        EmailAddress? toEmailAddress,
        PhoneNumber? fromPhoneNumber,
        PhoneNumber? toPhoneNumber,
        string? subject)
    {
        Channel = channel;
        Body = body;
        FromEmailAddress = fromEmailAddress;
        ToEmailAddress = toEmailAddress;
        FromPhoneNumber = fromPhoneNumber;
        ToPhoneNumber = toPhoneNumber;
        Subject = subject;
    }

    public static Result<Notification> TryCreate(INotificationDto dto)
    {
        if (!MessageBody.TryCreate(dto.Body, out var body))
            return Result<Notification>.Failure(NotificationMessages.InvalidMessageBodyErrorMessage);

        Notification notification;
        switch (dto.Channel)
        {
            case ChannelType.Email:
                if (!EmailAddress.TryCreate(dto.Recipient, out var toEmail))
                    return Result<Notification>.Failure(NotificationMessages.InvalidRecipientEmailAddressErrorMessage);

                if (!EmailAddress.TryCreate(dto.Sender, out var fromEmail))
                    return Result<Notification>.Failure(NotificationMessages.InvalidSenderEmailAddressErrorMessage);

                notification = new Notification(dto.Channel, body, fromEmail, toEmail, null, null, dto.Subject ?? "");
                return Result<Notification>.Success(notification);

            case ChannelType.Sms:
                if (!PhoneNumber.TryCreate(dto.Recipient, out var fromPhone))
                    return Result<Notification>.Failure(NotificationMessages.InvalidRecipientNumberPhoneErrorMessage);

                if (!PhoneNumber.TryCreate(dto.Sender, out var toPhone))
                    return Result<Notification>.Failure(NotificationMessages.InvalidSenderNumberPhoneErrorMessage);

                notification = new Notification(dto.Channel, body, null, null, fromPhone, toPhone, null);
                return Result<Notification>.Success(notification);

            case ChannelType.Push:
                notification = new Notification(dto.Channel, body, null, null, null, null, null);
                return Result<Notification>.Success(notification);

            default:
                return Result<Notification>.Failure($"Unsupported channel: {dto.Channel}");
        }
    }
}
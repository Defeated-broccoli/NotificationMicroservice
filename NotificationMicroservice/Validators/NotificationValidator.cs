using NotificationMicroservice.Entities;
using NotificationMicroservice.Enums;
using NotificationMicroservice.Interfaces;
using NotificationMicroservice.Models;

namespace NotificationMicroservice.Validators;

public class NotificationValidator : IValidator<NotificationDto>
{
    public ValidationResult Validate(NotificationDto entity)
    {
        if (entity == null)
        {
            return new ValidationResult(false, "Notification cannot be null");
        }

        if (string.IsNullOrWhiteSpace(entity.Message))
            return new ValidationResult(false, "Message cannot be empty");

        switch (entity.Channel)
        {
            case ChannelType.Email:
                if (!new System.ComponentModel.DataAnnotations.EmailAddressAttribute().IsValid(entity.Recipient))
                {
                    return new ValidationResult(false, "Invalid recipient email address");
                }

                if (!new System.ComponentModel.DataAnnotations.EmailAddressAttribute().IsValid(entity.Sender))
                {
                    return new ValidationResult(false, "Invalid sender email address");
                }

                break;

            case ChannelType.Sms:
                if (!new System.ComponentModel.DataAnnotations.PhoneAttribute().IsValid(entity.Recipient))
                {
                    return new ValidationResult(false, "Invalid recipient phone number");
                }

                if (!new System.ComponentModel.DataAnnotations.PhoneAttribute().IsValid(entity.Sender))
                {
                    return new ValidationResult(false, "Invalid sender phone number");
                }

                break;
        }

        return new ValidationResult(true);
    }
}
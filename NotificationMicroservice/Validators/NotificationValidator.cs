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

        return entity.Channel switch
        {
            ChannelType.Email =>
                new System.ComponentModel.DataAnnotations.EmailAddressAttribute().IsValid(entity.Recipient)
                    ? new ValidationResult(true)
                    : new ValidationResult(false, "Invalid email address"),

            ChannelType.Sms =>
                new System.ComponentModel.DataAnnotations.PhoneAttribute().IsValid(entity.Recipient)
                    ? new ValidationResult(true)
                    : new ValidationResult(false, "Invalid phone number"),

            ChannelType.Push => new ValidationResult(true),

            _ => new ValidationResult(false, "Unsupported channel type")
        };
    }
}
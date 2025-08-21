using NotificationMicroservice.Abstractions;
using NotificationMicroservice.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace NotificationMicroservice.Api.Dtos;

public class NotificationDto : INotificationDto
{
    public string Body { get; set; }

    public ChannelType Channel { get; set; }

    public string Recipient { get; set; }

    public string Sender { get; set; }

    public string? Subject { get; set; }
}
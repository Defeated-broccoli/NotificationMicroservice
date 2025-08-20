using NotificationMicroservice.Enums;
using System.ComponentModel.DataAnnotations;

namespace NotificationMicroservice.Entities;

public class NotificationDto
{
    public ChannelType Channel { get; set; }

    public string Message { get; set; }

    public string Recipient { get; set; }

    public string Sender { get; set; }
}
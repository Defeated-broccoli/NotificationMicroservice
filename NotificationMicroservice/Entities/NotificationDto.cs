using NotificationMicroservice.Enums;

namespace NotificationMicroservice.Entities;

public class NotificationDto
{
    public ChannelType Channel { get; set; }
    public string Message { get; set; }
    public string Recipient { get; set; }
}
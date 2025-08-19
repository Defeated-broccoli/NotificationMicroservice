using NotificationMicroservice.Enums;

namespace NotificationMicroservice.Entities;

public class Notification
{
    public Notification(string recipient, string message, ChannelType channel)
    {
        Recipient = recipient;
        Message = message;
        Channel = channel;
    }

    public ChannelType Channel { get; set; }
    public string Message { get; set; }
    public string Recipient { get; set; }
}
using NotificationMicroservice.Enums;

namespace NotificationMicroservice.Entities;

public class Notification
{
    public Notification(string recipient, string message, ChannelType channel)
    {
        Id = Guid.NewGuid().ToString();
        Recipient = recipient;
        Message = message;
        Channel = channel;
    }

    public ChannelType Channel { get; init; }
    public string Id { get; init; }
    public string Message { get; init; }
    public string Recipient { get; init; }
}
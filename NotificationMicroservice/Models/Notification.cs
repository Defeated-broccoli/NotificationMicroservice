using NotificationMicroservice.Enums;

namespace NotificationMicroservice.Entities;

public class Notification
{
    public Notification(string recipient, string message, ChannelType channel, string sender)
    {
        Id = Guid.NewGuid().ToString();
        Recipient = recipient;
        Message = message;
        Channel = channel;
        Sender = sender;
    }

    public ChannelType Channel { get; init; }

    public string Id { get; init; }

    public string Message { get; init; }

    public string Recipient { get; init; }

    public string Sender { get; set; }
}
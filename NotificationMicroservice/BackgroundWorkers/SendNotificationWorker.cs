using Amazon.SQS;
using Amazon.SQS.Model;
using NotificationMicroservice.Entities;
using NotificationMicroservice.Enums;
using NotificationMicroservice.Interfaces;

namespace NotificationMicroservice.BackgroundWorkers;

public class SendNotificationWorker : IBackgroundWorker
{
    private readonly IAmazonSQS _amazonSqs;
    private readonly Dictionary<ChannelType, IChannelHandler> _handlers;

    private string _queueUrl;

    public SendNotificationWorker(IAmazonSQS amazonSqs, IEnumerable<IChannelHandler> handlers, IConfiguration configuration)
    {
        _amazonSqs = amazonSqs;
        _handlers = handlers.ToDictionary(h => h.SupportedChannel);
        _queueUrl = configuration["AWS:QueueUrl"] ?? throw new Exception("AWS:QueueUrl is not set.");
    }

    public async Task ExecuteAsync()
    {
        var request = new ReceiveMessageRequest
        {
            QueueUrl = _queueUrl,
            MaxNumberOfMessages = 5,
            WaitTimeSeconds = 5
        };

        ReceiveMessageResponse response;
        try
        {
            response = await _amazonSqs.ReceiveMessageAsync(request);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error receiving messages from SQS: {ex.Message}");
            return;
        }

        foreach (var message in response.Messages)
        {
            try
            {
                var notification = System.Text.Json.JsonSerializer.Deserialize<Notification>(message.Body);
                if (notification == null)
                {
                    Console.WriteLine($"Failed to deserialize message {message.MessageId}");
                    continue;
                }

                if (_handlers.TryGetValue(notification.Channel, out var handler))
                {
                    var success = await handler.SendAsync(notification);
                    if (success)
                    {
                        await _amazonSqs.DeleteMessageAsync(_queueUrl, message.ReceiptHandle);
                    }
                    else
                    {
                        Console.WriteLine($"Handler failed for message {message.MessageId}, will retry later");
                    }
                }
                else
                {
                    Console.WriteLine($"No handler for channel {notification.Channel}, message {message.MessageId}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing message {message.MessageId}: {ex.Message}");
            }
        }
    }
}
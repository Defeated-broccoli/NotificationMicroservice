using Amazon.SQS;
using Amazon.SQS.Model;
using NotificationMicroservice.Application.Interfaces;
using NotificationMicroservice.Domain.Enums;
using NotificationMicroservice.Entities;
using NotificationMicroservice.Infrastructure.Dtos;
using NotificationMicroservice.Interfaces;

namespace NotificationMicroservice.Infrastructure.BackgroundWorkers;

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

        if (response.Messages == null || response.Messages.Count == 0)
        {
            return;
        }

        foreach (var message in response.Messages)
        {
            try
            {
                var notificationSqsDto = System.Text.Json.JsonSerializer.Deserialize<NotificationSqsDto>(message.Body);
                if (notificationSqsDto == null)
                {
                    Console.WriteLine($"Failed to deserialize message {message.MessageId}");
                    continue;
                }

                var validationResult = Notification.TryCreate(notificationSqsDto);
                if (!validationResult.IsValid)
                {
                    Console.WriteLine($"Invalid notification data in message {message.MessageId}: {validationResult.ErrorMessage}");
                    continue;
                }

                var notification = validationResult.Value!;
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
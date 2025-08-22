using Amazon.SQS;
using Amazon.SQS.Model;
using NotificationMicroservice.Application.Interfaces;
using NotificationMicroservice.Entities;
using NotificationMicroservice.Infrastructure.Mappers;

namespace NotificationMicroservice.Application.Services;

public class QueueService : IQueueService
{
    private readonly IAmazonSQS _amazonSQS;
    private readonly string _queueUrl;

    public QueueService(IAmazonSQS amazonSQS, IConfiguration configuration)
    {
        _amazonSQS = amazonSQS;
        _queueUrl = configuration["AWS:QueueUrl"] ?? throw new Exception("AWS:QueueUrl is not set.");
    }

    public async Task<bool> EnqueueNotification(Notification notification)
    {
        var notificationSqs = NotificationMapper.ToSqsDto(notification);

        try
        {
            var response = await _amazonSQS.SendMessageAsync(new SendMessageRequest
            {
                QueueUrl = _queueUrl,
                MessageBody = System.Text.Json.JsonSerializer.Serialize(notificationSqs)
            });

            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error sending message to SQS: {ex.Message}");
            return false;
        }
    }
}
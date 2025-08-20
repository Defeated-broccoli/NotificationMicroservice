using Amazon.SQS;
using Amazon.SQS.Model;
using NotificationMicroservice.Entities;
using NotificationMicroservice.Interfaces;

namespace NotificationMicroservice.Services;

public class QueueService : IQueueService
{
    private readonly IAmazonSQS _amazonSQS;

    private string _queueUrl = Environment.GetEnvironmentVariable("AWS_QUEUE_URL")
        ?? throw new InvalidOperationException("AWS_QUEUE_URL not set");

    public QueueService(IAmazonSQS amazonSQS)
    {
        _amazonSQS = amazonSQS;
    }

    public async Task<bool> EnqueueMessage(Notification notification)
    {
        try
        {
            var response = await _amazonSQS.SendMessageAsync(new SendMessageRequest
            {
                QueueUrl = _queueUrl,
                MessageBody = System.Text.Json.JsonSerializer.Serialize(notification)
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
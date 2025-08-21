using Amazon.SQS;
using Amazon.SQS.Model;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using NotificationMicroservice.Application.Interfaces;
using NotificationMicroservice.Application.Services;
using NotificationMicroservice.Infrastructure.Mappers;
using NotificationMicroserviceTests.TestCommons;
using System.Text.Json;

namespace NotificationMicroserviceTests.Application.Services.QueueServiceTests;

public class QueueServiceTests_EnqueueMessage : BaseTest
{
    private readonly Mock<IAmazonSQS> _amazonSQSMock = new();
    private readonly Mock<IConfiguration> _configurationMock = new();
    private readonly IQueueService _queueService;
    private readonly string _queueUrl = "https://url.com";

    public QueueServiceTests_EnqueueMessage()
    {
        _configurationMock.Setup(c => c["AWS:QueueUrl"])
            .Returns(_queueUrl);

        _queueService = new QueueService(
            _amazonSQSMock.Object,
            _configurationMock.Object
        );
    }

    [Fact]
    public async Task EnqueueMessage_ShouldReturnFalse_WhenMessageSendingFails()
    {
        // arrange
        var notification = CreateNotification();
        var notificationSqs = NotificationMapper.ToSqsDto(notification);
        _amazonSQSMock.Setup(s => s.SendMessageAsync(
            It.Is<SendMessageRequest>(r =>
            r.MessageBody == JsonSerializer.Serialize(
                    notificationSqs,
                    null as JsonSerializerOptions
                ) && r.QueueUrl == _queueUrl
            ), default)
        ).ThrowsAsync(new Exception("SQS error"));

        // act
        var result = await _queueService.EnqueueMessage(notification);

        // assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task EnqueueMessage_ShouldReturnTrue_WhenMessageIsSentSuccessfully()
    {
        // arrange
        var notification = CreateNotification();
        var notificationSqs = NotificationMapper.ToSqsDto(notification);

        _amazonSQSMock.Setup(s => s.SendMessageAsync(
            It.Is<SendMessageRequest>(r =>
            r.MessageBody == JsonSerializer.Serialize(
                    notificationSqs,
                    null as JsonSerializerOptions
                ) && r.QueueUrl == _queueUrl
            ), default)
        ).ReturnsAsync(new SendMessageResponse
        {
            HttpStatusCode = System.Net.HttpStatusCode.OK
        }
        );

        // act
        var result = await _queueService.EnqueueMessage(notification);

        // assert
        result.Should().BeTrue();
    }
}
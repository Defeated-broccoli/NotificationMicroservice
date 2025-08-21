using Amazon.SQS;
using Amazon.SQS.Model;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using NotificationMicroservice.Application.Interfaces;
using NotificationMicroservice.Domain.Enums;
using NotificationMicroservice.Entities;
using NotificationMicroservice.Infrastructure.BackgroundWorkers;
using NotificationMicroservice.Infrastructure.Dtos;

namespace NotificationMicroserviceTests.Infrastructure.BackgroundWorkers.SendNotificationWorkerTests;

public class SendNotificationWorkerTests
{
    private readonly Mock<IConfiguration> _configMock = new();
    private readonly Mock<IChannelHandler> _handlerMock = new();
    private readonly Mock<IAmazonSQS> _sqsMock = new();
    private readonly SendNotificationWorker _worker;

    public SendNotificationWorkerTests()
    {
        _configMock.Setup(c => c["AWS:QueueUrl"])
            .Returns("https://queue.url");

        _handlerMock.SetupGet(h => h.SupportedChannel)
            .Returns(ChannelType.Email);
        _handlerMock.Setup(h => h.SendAsync(It.IsAny<Notification>()))
            .ReturnsAsync(true);

        _worker = new SendNotificationWorker(
            _sqsMock.Object,
            [_handlerMock.Object],
            _configMock.Object
        );
    }

    [Fact]
    public async Task ExecuteAsync_WhenMessageIsInvalid_LogsErrorAndDoesNotCallHandler()
    {
        var invalidMessage = new Message
        {
            MessageId = "1",
            Body = "invalid-json"
        };

        _sqsMock.Setup(s => s.ReceiveMessageAsync(It.IsAny<ReceiveMessageRequest>(), default))
            .ReturnsAsync(new ReceiveMessageResponse { Messages = new List<Message> { invalidMessage } });

        var act = async () => await _worker.ExecuteAsync();

        await act.Should().NotThrowAsync();
        _handlerMock.Verify(h => h.SendAsync(It.IsAny<Notification>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_WhenNoMessages_DoesNotThrow()
    {
        _sqsMock.Setup(s => s.ReceiveMessageAsync(It.IsAny<ReceiveMessageRequest>(), default))
            .ReturnsAsync(new ReceiveMessageResponse { Messages = new List<Message>() });

        var act = async () => await _worker.ExecuteAsync();

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task ExecuteAsync_WhenValidMessage_CallsHandlerAndDeletesMessage()
    {
        var dto = new NotificationSqsDto
        {
            Channel = ChannelType.Email,
            Recipient = "to@email.com",
            Sender = "from@email.com",
            Body = "Test",
            Id = "123"
        };
        var message = new Message
        {
            MessageId = "1",
            Body = System.Text.Json.JsonSerializer.Serialize(dto),
            ReceiptHandle = "receipt"
        };

        _sqsMock.Setup(s => s.ReceiveMessageAsync(It.IsAny<ReceiveMessageRequest>(), default))
            .ReturnsAsync(new ReceiveMessageResponse { Messages = new List<Message> { message } });

        var act = async () => await _worker.ExecuteAsync();

        await act.Should().NotThrowAsync();
        _handlerMock.Verify(h => h.SendAsync(It.IsAny<Notification>()), Times.Once);
        _sqsMock.Verify(s => s.DeleteMessageAsync(It.IsAny<string>(), message.ReceiptHandle, default), Times.Once);
    }
}
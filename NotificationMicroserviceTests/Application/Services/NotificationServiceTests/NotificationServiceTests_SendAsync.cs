using FluentAssertions;
using Moq;
using NotificationMicroservice.Application.Interfaces;
using NotificationMicroservice.Application.Services;
using NotificationMicroserviceTests.TestCommons;

namespace NotificationMicroserviceTests.Application.Services.NotificationServiceTests;

public class NotificationServiceTests_SendAsync : BaseTest
{
    private readonly INotificationService _notificationService;

    private readonly Mock<IQueueService> _queueServiceMock = new();

    public NotificationServiceTests_SendAsync()
    {
        _notificationService = new NotificationService(_queueServiceMock.Object);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task SendAsync_ShouldReturnTrue_WhenEnqueueMessageSucceeds(bool isSuccess)
    {
        // arrange
        var notification = CreateNotification();
        _queueServiceMock
            .Setup(q => q.EnqueueMessage(notification))
            .ReturnsAsync(isSuccess);

        // act
        var result = await _notificationService.SendAsync(notification);

        // assert
        result.Should().Be(isSuccess);
    }
}
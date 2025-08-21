using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NotificationMicroservice.Api.Controllers;
using NotificationMicroservice.Application.Interfaces;
using NotificationMicroservice.Entities;
using NotificationMicroservice.Resources;
using NotificationMicroserviceTests.TestCommons;

namespace NotificationMicroserviceTests.Api.Controller.NotificationControllerTests;

public class NotificationControllerTests_SendNotificationTests : BaseTest
{
    private readonly NotificationsController _controller;
    private readonly Mock<INotificationService> _notificationServiceMock;

    public NotificationControllerTests_SendNotificationTests()
    {
        _notificationServiceMock = new Mock<INotificationService>();
        _controller = new NotificationsController(_notificationServiceMock.Object);
    }

    [Fact]
    public async Task SendNotification_ReturnsBadRequest_OnFailedEnqueue()
    {
        // Arrange
        var validDto = CreateNotificationDto();

        _notificationServiceMock
            .Setup(service => service.SendAsync(It.IsAny<Notification>()))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.SendNotification(validDto);

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var badRequestResult = (ObjectResult)result;
        badRequestResult.StatusCode.Should().Be(503);
        badRequestResult.Value.Should().BeEquivalentTo(new { message = NotificationMessages.FailedToSendNotification });
    }

    [Fact]
    public async Task SendNotification_ReturnsBadRequest_WhenDtoIsInvalid()
    {
        // Arrange
        var invalidDto = CreateNotificationDto(isValid: false);

        // Act
        var result = await _controller.SendNotification(invalidDto);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = (BadRequestObjectResult)result;
        badRequestResult.Value.Should().BeEquivalentTo(new { message = NotificationMessages.InvalidRecipientEmailAddressErrorMessage });
    }

    [Fact]
    public async Task SendNotification_ReturnsOK_OnSuccessfulEnqueue()
    {
        // Arrange
        var validDto = CreateNotificationDto();

        _notificationServiceMock
            .Setup(service => service.SendAsync(It.IsAny<Notification>()))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.SendNotification(validDto);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var badRequestResult = (OkObjectResult)result;
        badRequestResult.Value.Should().BeEquivalentTo(
            new
            {
                message = NotificationMessages.NotificationQueued
            }
        );
    }
}
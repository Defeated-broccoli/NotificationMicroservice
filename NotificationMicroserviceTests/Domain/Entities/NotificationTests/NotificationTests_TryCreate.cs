using FluentAssertions;
using NotificationMicroservice.Domain.Enums;
using NotificationMicroservice.Entities;
using NotificationMicroservice.Resources;
using NotificationMicroserviceTests.TestCommons;

namespace NotificationMicroserviceTests.Domain.Entities.NotificationTests;

public class NotificationTests_TryCreate : BaseTest
{
    [Theory]
    [InlineData(ChannelType.Email)]
    [InlineData(ChannelType.Sms)]
    [InlineData(ChannelType.Push)]
    public void TryCreate_ShouldCreateNotification_OnSuccessfulValidation(ChannelType channelType)
    {
        // arrange
        var notificationDto = CreateNotificationDto(channelType: channelType);

        // act
        var validationResult = Notification.TryCreate(notificationDto);

        // assert
        validationResult.ErrorMessage.Should().BeNull();
        validationResult.Value.Should().NotBeNull()
            .And.Match<Notification>(n => n.Channel == channelType);
    }

    [Fact]
    public void TryCreate_ShouldReturnErrorMessage_OnInvalidBody()
    {
        // arrange
        var notificationDto = CreateNotificationDto();
        notificationDto.Body = "";

        // act
        var validationResult = Notification.TryCreate(notificationDto);

        // assert
        validationResult.ErrorMessage.Should().Be(NotificationMessages.InvalidMessageBodyErrorMessage);
        validationResult.Value.Should().BeNull();
    }

    [Fact]
    public void TryCreate_ShouldReturnErrorMessage_OnInvalidRecipientEmail()
    {
        // arrange
        var notificationDto = CreateNotificationDto();
        notificationDto.Recipient = "invalidEmail";

        // act
        var validationResult = Notification.TryCreate(notificationDto);

        // assert
        validationResult.ErrorMessage.Should().Be(NotificationMessages.InvalidRecipientEmailAddressErrorMessage);
        validationResult.Value.Should().BeNull();
    }

    [Fact]
    public void TryCreate_ShouldReturnErrorMessage_OnInvalidRecipientPhone()
    {
        // arrange
        var notificationDto = CreateNotificationDto(channelType: ChannelType.Sms);
        notificationDto.Recipient = "invalidPhone";

        // act
        var validationResult = Notification.TryCreate(notificationDto);

        // assert
        validationResult.ErrorMessage.Should().Be(NotificationMessages.InvalidRecipientNumberPhoneErrorMessage);
        validationResult.Value.Should().BeNull();
    }

    [Fact]
    public void TryCreate_ShouldReturnErrorMessage_OnInvalidSenderEmail()
    {
        // arrange
        var notificationDto = CreateNotificationDto();
        notificationDto.Sender = "invalidEmail";

        // act
        var validationResult = Notification.TryCreate(notificationDto);

        // assert
        validationResult.ErrorMessage.Should().Be(NotificationMessages.InvalidSenderEmailAddressErrorMessage);
        validationResult.Value.Should().BeNull();
    }

    [Fact]
    public void TryCreate_ShouldReturnErrorMessage_OnInvalidSenderPhone()
    {
        // arrange
        var notificationDto = CreateNotificationDto(channelType: ChannelType.Sms);
        notificationDto.Sender = "invalidPhone";

        // act
        var validationResult = Notification.TryCreate(notificationDto);

        // assert
        validationResult.ErrorMessage.Should().Be(NotificationMessages.InvalidSenderNumberPhoneErrorMessage);
        validationResult.Value.Should().BeNull();
    }
}
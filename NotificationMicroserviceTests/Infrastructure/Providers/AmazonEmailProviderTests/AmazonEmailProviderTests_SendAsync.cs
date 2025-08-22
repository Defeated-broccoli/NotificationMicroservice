using Amazon.SimpleEmailV2;
using Amazon.SimpleEmailV2.Model;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using NotificationMicroservice.Infrastructure.Commons;
using NotificationMicroservice.Infrastructure.Interfaces;
using NotificationMicroservice.Infrastructure.Providers;
using NotificationMicroserviceTests.TestCommons;

namespace NotificationMicroserviceTests.Infrastructure.Providers.AmazonEmailProviderTests;

public class AmazonEmailProviderTests_SendAsync : BaseTest
{
    public readonly INotificationProvider _amazonEmailProvider;
    public readonly Mock<IAmazonSimpleEmailServiceV2> _sesServiceMock = new();
    private readonly Mock<IOptionsSnapshot<Dictionary<string, ProviderConfig>>> _options = new();

    public AmazonEmailProviderTests_SendAsync()
    {
        _options.Setup(o => o.Value).Returns(new Dictionary<string, ProviderConfig>
        {
            {
                "AmazonEmail", new ProviderConfig
                {
                    IsEnabled = true,
                    Name = "AmazonEmail",
                    Priority = 1
                }
            }
        });
        _amazonEmailProvider = new AmazonEmailProvider(_sesServiceMock.Object, _options.Object);
    }

    [Fact]
    public async Task SendAsync_ShouldReturnFalse_WhenEmailSendingFails()
    {
        // arrange
        var notification = CreateNotification();
        _sesServiceMock
            .Setup(x => x.SendEmailAsync(
                It.IsAny<SendEmailRequest>(),
                default
            ))
            .ThrowsAsync(new Exception("Email sending failed"));

        // act
        var result = await _amazonEmailProvider.SendAsync(notification);

        // assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task SendAsync_ShouldReturnTrue_WhenEmailIsSentSuccessfully()
    {
        // arrange
        var notification = CreateNotification();
        _sesServiceMock
            .Setup(x => x.SendEmailAsync(
                It.IsAny<SendEmailRequest>(),
                default
            ))
            .ReturnsAsync(new SendEmailResponse
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK
            });

        // act
        var result = await _amazonEmailProvider.SendAsync(notification);

        // assert
        _sesServiceMock.Verify(x => x.SendEmailAsync(
            It.Is<SendEmailRequest>(r =>
                r.FromEmailAddress == notification.FromEmailAddress
                && r.Destination.ToAddresses.Contains(notification.ToEmailAddress)
                && r.Content.Simple.Subject.Data == notification.Subject
                && r.Content.Simple.Body.Text.Data == notification.Body
            ),
            default
        ), Times.Once);
        result.Should().BeTrue();
    }
}
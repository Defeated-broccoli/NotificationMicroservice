using FluentAssertions;
using Moq;
using NotificationMicroservice.Application.Handlers;
using NotificationMicroservice.Domain.Enums;
using NotificationMicroservice.Infrastructure.Interfaces;
using NotificationMicroserviceTests.TestCommons;

namespace NotificationMicroserviceTests.Application.Handlers.EmailChannelHandlerTests;

public class EmailChannelHandlerTests_SendAsync : BaseTest
{
    private readonly BaseChannelHandler _emailChannelHandler;
    private readonly Mock<INotificationProvider> _emailProviderMockDisabled;
    private readonly Mock<INotificationProvider> _emailProviderMockNoPriority;
    private readonly Mock<INotificationProvider> _emailProviderMockPriority;
    private readonly IEnumerable<INotificationProvider> _providers;
    private readonly Mock<INotificationProvider> _smsProviderMock;

    public EmailChannelHandlerTests_SendAsync()
    {
        _emailProviderMockPriority = new Mock<INotificationProvider>();
        _emailProviderMockPriority.Setup(p => p.IsEnabled).Returns(true);
        _emailProviderMockPriority.Setup(p => p.Priority).Returns(0);
        _emailProviderMockPriority.Setup(p => p.SupportedChannel).Returns(ChannelType.Email);

        _emailProviderMockNoPriority = new Mock<INotificationProvider>();
        _emailProviderMockNoPriority.Setup(p => p.IsEnabled).Returns(true);
        _emailProviderMockNoPriority.Setup(p => p.Priority).Returns(100);
        _emailProviderMockNoPriority.Setup(p => p.SupportedChannel).Returns(ChannelType.Email);

        _smsProviderMock = new Mock<INotificationProvider>();
        _smsProviderMock.Setup(p => p.IsEnabled).Returns(true);
        _smsProviderMock.Setup(p => p.Priority).Returns(120);
        _smsProviderMock.Setup(p => p.SupportedChannel).Returns(ChannelType.Sms);

        _emailProviderMockDisabled = new Mock<INotificationProvider>();
        _emailProviderMockDisabled.Setup(p => p.IsEnabled).Returns(false);
        _emailProviderMockDisabled.Setup(p => p.Priority).Returns(100);
        _emailProviderMockDisabled.Setup(p => p.SupportedChannel).Returns(ChannelType.Email);

        _providers =
        [
            _emailProviderMockPriority.Object,
            _emailProviderMockNoPriority.Object,
            _smsProviderMock.Object,
            _emailProviderMockDisabled.Object
        ];

        _emailChannelHandler = new EmailChannelHandler(_providers);
    }

    [Fact]
    public async Task SendAsync_ShouldReturnFalse_WhenAllProvidersFailed()
    {
        // arrange
        var notification = CreateNotification();
        _emailProviderMockPriority.Setup(p => p.SendAsync(notification)).ReturnsAsync(false);
        _emailProviderMockNoPriority.Setup(p => p.SendAsync(notification)).ReturnsAsync(false);

        // act
        var result = await _emailChannelHandler.SendAsync(notification);

        // assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task SendAsync_ShouldReturnTrue_AfterSuccessfulSend()
    {
        // arrange
        var notification = CreateNotification();
        _emailProviderMockPriority.Setup(p => p.SendAsync(notification)).ReturnsAsync(true);

        // act
        var result = await _emailChannelHandler.SendAsync(notification);

        // assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task SendAsync_ShouldSelectCorrectProviders()
    {
        // arrange
        var notification = CreateNotification();

        // act
        await _emailChannelHandler.SendAsync(notification);

        // assert
        _emailProviderMockPriority.Verify(p => p.SendAsync(notification), Times.Once);
        _emailProviderMockNoPriority.Verify(p => p.SendAsync(notification), Times.Once);
        _smsProviderMock.Verify(p => p.SendAsync(notification), Times.Never);
        _emailProviderMockDisabled.Verify(p => p.SendAsync(notification), Times.Never);
    }
}
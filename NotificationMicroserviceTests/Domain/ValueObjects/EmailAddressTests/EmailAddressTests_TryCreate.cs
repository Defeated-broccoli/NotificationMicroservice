using FluentAssertions;
using NotificationMicroservice.Domain.ValueObjects;

namespace NotificationMicroserviceTests.Domain.ValueObjects.EmailAddressTests;

public class EmailAddressTests_TryCreate
{
    [Theory]
    [InlineData("email@valid.com", true, "email@valid.com")]
    [InlineData("   email@valid.com   ", true, "email@valid.com")]
    [InlineData("email.com", false, null)]
    [InlineData("  ", false, null)]
    [InlineData(null, false, null)]
    public void TryCreate_ShouldReturnExpectedResult(string input, bool expectedSuccess, string? expectedValue)
    {
        // Act
        var result = EmailAddress.TryCreate(input, out var email);

        // Assert
        result.Should().Be(expectedSuccess);

        if (expectedSuccess)
        {
            email.Value.Should().Be(expectedValue);
        }
        else
        {
            email.Value.Should().BeNull();
        }
    }
}
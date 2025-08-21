using FluentAssertions;
using NotificationMicroservice.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationMicroserviceTests.Domain.ValueObjects.PhoneNumberTests;

public class PhoneNumberTests_TryCreate
{
    [Theory]
    [InlineData("+1234567890", true, "+1234567890")]
    [InlineData("   +1234567890   ", true, "+1234567890")]
    [InlineData("abc123", false, null)]
    [InlineData("", false, null)]
    [InlineData(null, false, null)]
    public void TryCreate_ShouldReturnExpectedResult(string? input, bool expectedSuccess, string? expectedValue)
    {
        // Act
        var result = PhoneNumber.TryCreate(input, out var phoneNumber);

        // Assert
        result.Should().Be(expectedSuccess);

        if (expectedSuccess)
            phoneNumber.Value.Should().Be(expectedValue);
        else
            phoneNumber.Value.Should().BeNull();
    }
}
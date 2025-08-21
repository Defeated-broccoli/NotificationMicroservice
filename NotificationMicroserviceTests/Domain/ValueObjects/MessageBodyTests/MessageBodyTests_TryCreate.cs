using FluentAssertions;
using NotificationMicroservice.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationMicroserviceTests.Domain.ValueObjects.MessageBodyTests;

public class MessageBodyTests_TryCreate
{
    public static IEnumerable<object[]> MessageBodyTestData =>
        [
            ["Hello world", true, "Hello world"],
            ["   Trim this   ", true, "Trim this"],
            ["", false, null],
            ["  ", false, null],
            [null, false, null],
            [new string('x', 4001), false, null]
        ];

    [Theory]
    [MemberData(nameof(MessageBodyTestData))]
    public void TryCreate_ShouldReturnExpectedResult(string? input, bool expectedSuccess, string? expectedValue)
    {
        var result = MessageBody.TryCreate(input, out var body);

        result.Should().Be(expectedSuccess);

        if (expectedSuccess)
            body.Value.Should().Be(expectedValue);
        else
            body.Value.Should().BeNull();
    }
}
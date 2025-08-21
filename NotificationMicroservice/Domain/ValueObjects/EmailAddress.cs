using System.ComponentModel.DataAnnotations;

namespace NotificationMicroservice.Domain.ValueObjects;

public readonly record struct EmailAddress
{
    public string Value { get; }
    private EmailAddress(string value) => Value = value;

    public static bool TryCreate(string? input, out EmailAddress email)
    {
        email = default;

        if (string.IsNullOrWhiteSpace(input))
        {
            return false;
        }

        var trimmedInput = input.Trim();

        if (!new EmailAddressAttribute().IsValid(trimmedInput))
        {
            return false;
        }

        email = new EmailAddress(trimmedInput);
        return true;
    }

    public static implicit operator string(EmailAddress email) => email.Value;
}
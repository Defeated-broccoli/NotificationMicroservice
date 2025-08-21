using System.ComponentModel.DataAnnotations;

namespace NotificationMicroservice.Domain.ValueObjects;

public readonly record struct PhoneNumber
{
    public string Value { get; }
    private PhoneNumber(string value) => Value = value;

    public static bool TryCreate(string? input, out PhoneNumber phoneNumber)
    {
        phoneNumber = default;

        if (string.IsNullOrWhiteSpace(input))
        {
            return false;
        }

        var trimmedInput = input.Trim();

        if (!new PhoneAttribute().IsValid(trimmedInput))
        {
            return false;
        }

        phoneNumber = new PhoneNumber(trimmedInput);
        return true;
    }

    public static implicit operator string(PhoneNumber p) => p.Value;
}
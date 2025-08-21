namespace NotificationMicroservice.Domain.ValueObjects;

public readonly record struct MessageBody
{
    public string Value { get; }

    private MessageBody(string value) => Value = value;

    public static bool TryCreate(string? input, out MessageBody body, int min = 1, int max = 4000)
    {
        body = default;

        if (string.IsNullOrWhiteSpace(input))
        {
            return false;
        }

        var trimmedInput = input.Trim();

        if (trimmedInput.Length < min || trimmedInput.Length > max)
        {
            return false;
        }

        body = new MessageBody(trimmedInput);
        return true;
    }

    public static implicit operator string(MessageBody m) => m.Value;
}
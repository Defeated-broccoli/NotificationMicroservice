namespace NotificationMicroservice.Domain.Common;

public class Result<T>
{
    public string? ErrorMessage { get; }

    public bool IsValid { get; }

    public T? Value { get; }

    private Result(T value)
    {
        IsValid = true;
        Value = value;
    }

    private Result(string errorMessage)
    {
        IsValid = false;
        ErrorMessage = errorMessage;
    }

    public static Result<T> Failure(string error) => new(error);

    public static Result<T> Success(T value) => new(value);
}
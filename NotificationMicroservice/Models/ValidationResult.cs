namespace NotificationMicroservice.Models;

public class ValidationResult
{
    public ValidationResult(bool isSuccess, string? errorMessage = null)
    {
        IsValid = isSuccess;
        ErrorMessage = errorMessage;
    }

    public string? ErrorMessage { get; set; }

    public bool IsValid { get; set; }
}
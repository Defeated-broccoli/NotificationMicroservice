using NotificationMicroservice.Models;

namespace NotificationMicroservice.Interfaces;

public interface IValidator<T>
{
    ValidationResult Validate(T entity);
}
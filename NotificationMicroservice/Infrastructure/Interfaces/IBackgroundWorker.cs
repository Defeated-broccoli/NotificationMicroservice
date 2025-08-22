namespace NotificationMicroservice.Infrastructure.Interfaces;

public interface IBackgroundWorker
{
    Task ExecuteAsync();
}
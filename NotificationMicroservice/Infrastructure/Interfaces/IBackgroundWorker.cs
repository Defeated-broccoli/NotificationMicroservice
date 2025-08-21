namespace NotificationMicroservice.Interfaces;

public interface IBackgroundWorker
{
    Task ExecuteAsync();
}
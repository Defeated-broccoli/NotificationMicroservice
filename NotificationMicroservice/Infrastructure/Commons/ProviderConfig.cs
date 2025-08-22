namespace NotificationMicroservice.Infrastructure.Commons;

public class ProviderConfig
{
    public bool IsEnabled { get; set; }

    public string? Name { get; set; }

    public int Priority { get; set; }
}
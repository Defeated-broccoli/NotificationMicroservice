using Amazon.SimpleEmailV2;
using Amazon.SimpleEmailV2.Model;
using Microsoft.Extensions.Options;
using NotificationMicroservice.Domain.Enums;
using NotificationMicroservice.Entities;
using NotificationMicroservice.Infrastructure.Commons;
using NotificationMicroservice.Infrastructure.Interfaces;

namespace NotificationMicroservice.Infrastructure.Providers;

public class AmazonEmailProvider : INotificationProvider
{
    private readonly ProviderConfig _providerConfig;
    private readonly IAmazonSimpleEmailServiceV2 _sesService;

    public bool IsEnabled => _providerConfig.IsEnabled;

    public string? Name => _providerConfig.Name;

    public int Priority => _providerConfig.Priority;

    public ChannelType SupportedChannel => ChannelType.Email;

    public AmazonEmailProvider(IAmazonSimpleEmailServiceV2 sesService, IOptionsSnapshot<Dictionary<string, ProviderConfig>> options)
    {
        _sesService = sesService;
        _providerConfig = options.Value["AmazonEmail"];
    }

    public async Task<bool> SendAsync(Notification notification)
    {
        try
        {
            var request = new SendEmailRequest
            {
                FromEmailAddress = notification.FromEmailAddress,
                Destination = new Destination
                {
                    ToAddresses = [notification.ToEmailAddress]
                },
                Content = new EmailContent
                {
                    Simple = new Message
                    {
                        Subject = new Content
                        {
                            Data = notification.Subject,
                        },
                        Body = new Body
                        {
                            Text = new Content
                            {
                                Data = notification.Body,
                            }
                        }
                    }
                }
            };

            var response = await _sesService.SendEmailAsync(request);
            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error sending email: {ex.Message}");
            return false;
        }
    }
}
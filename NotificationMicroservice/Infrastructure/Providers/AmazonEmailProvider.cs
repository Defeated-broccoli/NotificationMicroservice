using Amazon.SimpleEmailV2;
using Amazon.SimpleEmailV2.Model;
using NotificationMicroservice.Domain.Enums;
using NotificationMicroservice.Entities;
using NotificationMicroservice.Infrastructure.Interfaces;

namespace NotificationMicroservice.Infrastructure.Providers;

public class AmazonEmailProvider : INotificationProvider
{
    private readonly IAmazonSimpleEmailServiceV2 _sesService;

    public bool IsEnabled => true;

    public string Name => "AmazonEmail";

    public int Priority => 1;

    public ChannelType SupportedChannel => ChannelType.Email;

    public AmazonEmailProvider(IAmazonSimpleEmailServiceV2 sesService)
    {
        _sesService = sesService;
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
using Amazon.SimpleEmailV2;
using Amazon.SimpleEmailV2.Model;
using NotificationMicroservice.Entities;
using NotificationMicroservice.Enums;
using NotificationMicroservice.Interfaces;

namespace NotificationMicroservice.Providers;

public class AmazonEmailProvider : INotificationProvider
{
    private readonly IAmazonSimpleEmailServiceV2 _sesService;

    public AmazonEmailProvider(IAmazonSimpleEmailServiceV2 sesService)
    {
        _sesService = sesService;
    }

    public bool IsEnabled => true;

    public string Name => "AmazonEmail";

    public int Priority => 1;

    public ChannelType SupportedChannel => ChannelType.Email;

    public async Task<bool> SendAsync(Notification notification)
    {
        try
        {
            var request = new SendEmailRequest
            {
                FromEmailAddress = "example@gmail.com",
                Destination = new Destination
                {
                    ToAddresses = [notification.Recipient]
                },
                Content = new EmailContent
                {
                    Simple = new Message
                    {
                        Subject = new Content
                        {
                            Data = "Notification"
                        },
                        Body = new Body
                        {
                            Text = new Content
                            {
                                Data = notification.Message,
                            }
                        }
                    }
                }
            };

            var response = await _sesService.SendEmailAsync(request);
            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }
        catch
        {
            return false;
        }
    }
}
using Amazon.SimpleEmailV2;
using Amazon.SQS;
using Hangfire;
using Hangfire.MemoryStorage;
using NotificationMicroservice.Application.Handlers;
using NotificationMicroservice.Application.Interfaces;
using NotificationMicroservice.Application.Services;
using NotificationMicroservice.Infrastructure.BackgroundWorkers;
using NotificationMicroservice.Infrastructure.Interfaces;
using NotificationMicroservice.Infrastructure.Providers;

namespace NotificationMicroservice
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<IQueueService, QueueService>();

            builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
            builder.Services.AddAWSService<IAmazonSQS>();
            builder.Services.AddAWSService<IAmazonSimpleEmailServiceV2>();

            // handlers
            builder.Services.AddScoped<IChannelHandler, EmailChannelHandler>();
            builder.Services.AddScoped<IChannelHandler, SmsChannelHandler>();
            builder.Services.AddScoped<IChannelHandler, PushChannelHandler>();

            // providers
            builder.Services.AddScoped<INotificationProvider, TwilioSmsProvider>();
            builder.Services.AddScoped<INotificationProvider, TwilioEmailProvider>();
            builder.Services.AddScoped<INotificationProvider, TwilioPushProvider>();

            builder.Services.AddScoped<INotificationProvider, AmazonSmsProvider>();
            builder.Services.AddScoped<INotificationProvider, AmazonEmailProvider>();
            builder.Services.AddScoped<INotificationProvider, AmazonPushProvider>();

            // background workers
            builder.Services.AddScoped<SendNotificationWorker>();

            // hangfire
            builder.Services.AddHangfire(config =>
            {
                config.UseMemoryStorage();
            });
            builder.Services.AddHangfireServer();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            using (var scope = app.Services.CreateScope())
            {
                var recurringJobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
                var worker = scope.ServiceProvider.GetRequiredService<SendNotificationWorker>();

                recurringJobManager.AddOrUpdate(
                    "send-notifications-job",
                    () => worker.ExecuteAsync(),
                    "*/2 * * * *"
                );
            }

            app.Run();
        }
    }
}
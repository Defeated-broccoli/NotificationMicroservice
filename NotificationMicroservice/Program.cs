using NotificationMicroservice.Handlers;
using NotificationMicroservice.Interfaces;
using NotificationMicroservice.Providers;
using NotificationMicroservice.Service;

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

            builder.Services.AddScoped<NotificationService>();

            builder.Services.AddScoped<IChannelHandler, EmailChannelHandler>();
            builder.Services.AddScoped<IChannelHandler, SmsChannelHandler>();
            builder.Services.AddScoped<IChannelHandler, PushChannelHandler>();

            builder.Services.AddScoped<INotificationProvider, TwilioSmsProvider>();
            builder.Services.AddScoped<INotificationProvider, TwilioEmailProvider>();
            builder.Services.AddScoped<INotificationProvider, TwilioPushProvider>();

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

            app.Run();
        }
    }
}
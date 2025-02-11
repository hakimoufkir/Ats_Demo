namespace Ats_Demo.Application.Services.IMessaging
{
    public interface IAzureServiceBusConsumer
    {
        Task StartProcessingAsync();
    }
}
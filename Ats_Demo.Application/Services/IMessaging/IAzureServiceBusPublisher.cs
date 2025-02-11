namespace Ats_Demo.Application.Services.IMessaging
{
    public interface IAzureServiceBusPublisher
    {
        Task PublishMessageAsync<T>(T messageObject);
    }
}
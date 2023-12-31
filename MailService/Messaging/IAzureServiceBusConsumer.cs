namespace MailService.Messaging
{
    public interface IAzureServiceBusConsumer
    {
        Task Start(); //start checking if service bus and queue have messages
        Task Stop(); //stop checking
    }
}

using MailService.Messaging;

namespace MailService.Extensions
{
    public static class AzureServiceBusExtension
    {

        public static IAzureServiceBusConsumer azureServiceBusConsumer { get; set; } //add as a property as it is the interface implementing this class
        public static IApplicationBuilder useAzure(this IApplicationBuilder app)
        {

            //know about the Consumer service and also about app Lifetime i.e when app is starting and stopping

            azureServiceBusConsumer = app.ApplicationServices.GetService<IAzureServiceBusConsumer>(); //we want to know about this service.Must have first been registered in program.cs .Make one instance of it i.e singleton so we know which instance to track
          

            var HostLifetime = app.ApplicationServices.GetService<IHostApplicationLifetime>(); //to know when app is starting and stopping

            HostLifetime.ApplicationStarted.Register(OnAppStart);  //register a method that will handle the app starting
            HostLifetime.ApplicationStopping.Register(OnAppStopping); //when application is about to stop

            return app;  //ensure to return this or else application won't execute.

        }

        private static void OnAppStopping()
        {
            azureServiceBusConsumer.Stop();  //azureServiceBusConsumer calls the stop method in AzureServiceBusConsumer
        }

        private static void OnAppStart()
        {
            azureServiceBusConsumer.Start(); //azureServiceBusConsumer calls the start method in AzureServiceBusConsumer
        }
    }
}


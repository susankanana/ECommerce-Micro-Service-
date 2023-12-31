using Azure.Messaging.ServiceBus;
using MailService.Models;
using MailService.Models.Dtos;
using MailService.Services;
using Newtonsoft.Json;
using System.Globalization;
using System.Text;

namespace MailService.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly string _queueName;
        private readonly ServiceBusProcessor _emailProcessor;
        private readonly ServiceBusProcessor _orderProcessor;
        private readonly MailsService _emailService;  //sends email
        private readonly EmailService _email;  //saves to db

        public AzureServiceBusConsumer(IConfiguration configuration , EmailService service) 
        {
            _email = service;
            _configuration = configuration;
            _connectionString = _configuration.GetValue<string>("AzureConnectionString");
            _queueName = _configuration.GetValue<string>("QueueAndTopics:registerQueue");

            var client = new ServiceBusClient(_connectionString);
            _emailProcessor = client.CreateProcessor(_queueName);
            _orderProcessor = client.CreateProcessor("orderadded", "MailService");
            _emailService = new MailsService(configuration);

        }
        public async Task Start() //called whenever api starts
        {
            _emailProcessor.ProcessMessageAsync += OnRegisterUser;  //to start processing email go to this method
            _emailProcessor.ProcessErrorAsync += ErrorHandler;  //in case there's an error in accessing the queue
            await _emailProcessor.StartProcessingAsync(); //start processing message and remove them from queue

            _orderProcessor.ProcessMessageAsync += OnMakeOrder;
            _orderProcessor.ProcessErrorAsync += ErrorHandler;
            await _orderProcessor.StartProcessingAsync();
        }

        private async Task OnMakeOrder(ProcessMessageEventArgs args)
        {
            var message = args.Message; //to read a message in the event ie queue in Azure
            var body = Encoding.UTF8.GetString(message.Body);//read  as String
            var reward = JsonConvert.DeserializeObject<RewardsDto>(body);//string to RewardsDto

            try
            {

                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("<img src=\"https://cdn.pixabay.com/photo/2018/02/12/16/41/thank-3148710_1280.png\" width=\"1000\" height=\"600\">");
                stringBuilder.Append("<h1> Hello " + reward.Name + "</h1>");
                stringBuilder.AppendLine("<br/> Order Made Successfully ");

                stringBuilder.Append("<br/>");
                stringBuilder.Append('\n');
                stringBuilder.Append("<p>You can Make another Order!!</p>");

                var user = new UserMessageDto()
                {
                    Email = reward.Email,
                    Name = reward.Name,
                };
                await _emailService.sendEmail(user, stringBuilder.ToString(), "ECommerce order");


                //insert  to Database
                var emaiLLogger = new EmailLogger()
                {
                    Name = user.Name,
                    Email = user.Email,
                    Message = stringBuilder.ToString(),
                    DateTime = DateTime.Now,

                };
                await _email.addDatatoDatabase(emaiLLogger);

                await args.CompleteMessageAsync(args.Message);//we are done delete the message from the queue 
            }
            catch (Exception ex)
            {
                throw;
                //send an Email to Admin
            }
        }

        public async Task Stop() //called whenever the application is stopping
        {
            await _emailProcessor.StopProcessingAsync();
            await _emailProcessor.DisposeAsync(); //free the resources

            await _orderProcessor.StopProcessingAsync();
            await _orderProcessor.DisposeAsync(); //free the resources
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            //send Email to Admin 
            return Task.CompletedTask;
        }

        private async Task OnRegisterUser(ProcessMessageEventArgs args)
        {
            var message = args.Message; //to read a message in the event ie queue in Azure
            var body = Encoding.UTF8.GetString(message.Body);//read  as String
            var user = JsonConvert.DeserializeObject<UserMessageDto>(body);//string to UserMessageDto

            try
            {


                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("<img src=\"https://media.istockphoto.com/id/1405760376/vector/online-shopping-design-graphic-elements-signs-symbols-mobile-marketing-and-digital-marketing.jpg?s=612x612&w=0&k=20&c=2DSpkY9ktsAfzBOcZUMkZThW3B6kvGYG1cHQ3yeaPJg=\" width=\"1000\" height=\"600\">");
                stringBuilder.Append("<h1> Hello " + user.Name + "</h1>");
                stringBuilder.AppendLine("<br/>Welcome to Suzie's Ecommerce App :)");

                stringBuilder.Append("<br/>");
                stringBuilder.Append('\n');
                stringBuilder.Append("<p>Your one stop shop of choice. Shop till you drop!!</p>");
                stringBuilder.Append("<p>Earn points with every purchase and you might just get lucky.</p>");

                await _emailService.sendEmail(user, stringBuilder.ToString()); //receives user of type UserMessageDto and Message


                //insert  to Database
                var emaiLLogger = new EmailLogger()
                {
                    Name = user.Name,
                    Email = user.Email,
                    Message = stringBuilder.ToString(),
                    DateTime = DateTime.Now,

                };
                await _email.addDatatoDatabase(emaiLLogger);

                await args.CompleteMessageAsync(args.Message);//we are done delete the message from the queue 
            }
            catch (Exception ex)
            {
                throw;
                //send an Email to Admin
            }
        }


        
    }
}

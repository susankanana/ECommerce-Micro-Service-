using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using RewardsService.Models;
using RewardsService.Models.Dtos;
using RewardsService.Services;
using System.Text;

namespace RewardsService.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly string _topicName;
        private readonly string _subscription;
        private readonly ServiceBusProcessor _rewardsProcessor;
        private readonly RewardService _rewardService;


        public AzureServiceBusConsumer(IConfiguration configuration, RewardService reward)
        {
            _configuration = configuration;
            _rewardService = reward;
            _connectionString = _configuration.GetValue<string>("AzureConnectionString");
            _topicName = _configuration.GetValue<string>("QueueAndTopics:bookingTopic");
            _subscription = _configuration.GetValue<string>("QueueAndTopics:bookingSubscription");

            var client = new ServiceBusClient(_connectionString);
            _rewardsProcessor = client.CreateProcessor(_topicName, _subscription);

        }
        public async Task Start()
        {
            _rewardsProcessor.ProcessMessageAsync += OnMakingOrder;
            _rewardsProcessor.ProcessErrorAsync += ErrorHandler;
            await _rewardsProcessor.StartProcessingAsync();
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            //send Email to Admin 
            return Task.CompletedTask;
        }
        
        private async Task OnMakingOrder(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);//read  as String
            var reward = JsonConvert.DeserializeObject<RewardsDto>(body);//string to RewardsDto

            try
            {
                //insert  to Database
                var rwd = new Reward()
                {
                    OrderId = reward.OrderId,
                    OrderTotal = reward.OrderTotal,
                    Email = reward.Email,
                    Name = reward.Name,
                    Points = reward.OrderTotal / 1000

                };

                await _rewardService.AddReward(rwd);
                await args.CompleteMessageAsync(args.Message);//we are done delete the message from the queue 
            }
            catch (Exception ex)
            {
                throw;
                //send an Email to Admin
            }
        }

        public async Task Stop()
        {
            await _rewardsProcessor.StopProcessingAsync();
            await _rewardsProcessor.DisposeAsync();
        }
    }
}

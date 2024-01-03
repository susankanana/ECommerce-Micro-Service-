using Azure.Messaging.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceMessageBus
{
    public interface IMessageBus
    {
        public Task PublishMessage(object message, string Topic_Queue_Name);
        
    }
}

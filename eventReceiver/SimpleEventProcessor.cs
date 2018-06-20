using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace eventReceiver
{
    public struct Message
    {
        int orderId;
        string eventText;

        public Message(int orderId, string eventText)
        {
            this.orderId = orderId;
            this.eventText = eventText;
        }

    }

    public class SimpleEventProcessor : IEventProcessor
    {
        public Task CloseAsync(PartitionContext context, CloseReason reason)
        {
            Console.WriteLine($"Processor Shutting Down. Partition '{context.PartitionId}', Reason: '{reason}'.");
            return Task.CompletedTask;
        }

        public Task OpenAsync(PartitionContext context)
        {
            Console.WriteLine($"SimpleEventProcessor initialized. Partition: '{context.PartitionId}'");
            return Task.CompletedTask;
        }

        public Task ProcessErrorAsync(PartitionContext context, Exception error)
        {
            Console.WriteLine($"Error on Partition: {context.PartitionId}, Error: {error.Message}");
            return Task.CompletedTask;
        }

       
        public Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
        {
            foreach (var eventData in messages)
            {
                Console.WriteLine("aaaaa");
                //var data = Encoding.UTF8.GetString(eventData.Body.Array, eventData.Body.Offset, eventData.Body.Count);
                
                var jsonBody = Encoding.UTF8.GetString(eventData.Body.Array, eventData.Body.Offset, eventData.Body.Count);
                //var eventTypeName = (string)eventData.Properties["EventType"];
                //var eventType = Type.GetType(eventTypeName);
                var data1 = JsonConvert.DeserializeObject(jsonBody);
                JObject data = JObject.Parse(jsonBody);
                var a = data["orderId"];
                //var data = JsonConvert.DeserializeObject(jsonBody, eventType);

                Console.WriteLine($"Message received. Partition: '{context.PartitionId}' and '{eventData.Body};, Data: '{data}'"); 
              
                context.CheckpointAsync();
            }

            return Task.CompletedTask;
        }
    }
}

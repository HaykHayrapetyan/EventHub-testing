using System;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;
using System.Threading.Tasks;


namespace eventReceiver
{
    public class Program
    {
        private const string EhConnectionString = "Endpoint=sb://rating.servicebus.windows.net/;SharedAccessKeyName=default;SharedAccessKey=pnTREAwnbvmnoR5SdFo0Nsd7jkJoW9Abm5b6xrOqeVg=;";
        private const string EhEntityPath = "immidiaterating";
        private const string StorageContainerName = "host-49e555d3-dcbe-4ab0-a6f9-fced2fcbca77";
        private const string StorageAccountName = "ratingstorage";
        private const string StorageAccountKey = "DoragUmhI785UUOYoDCO+1n5C0mwmh37WPZFSztzcD5ENqlfF28Hyk+F5ujAVAolUxbMcmD49XRBkxRuXQlxyQ==";

        private static readonly string StorageConnectionString = string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", StorageAccountName, StorageAccountKey);

        public static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        private static async Task MainAsync(string[] args)
        {
            Console.WriteLine("Registering EventProcessor...");

            var eventProcessorHost = new EventProcessorHost(
                EhEntityPath,
                PartitionReceiver.DefaultConsumerGroupName,
                EhConnectionString,
                StorageConnectionString,
                StorageContainerName);

            // Registers the Event Processor Host and starts receiving messages
            await eventProcessorHost.RegisterEventProcessorAsync<SimpleEventProcessor>();

            Console.WriteLine("Receiving. Press ENTER to stop worker.");
            Console.ReadLine();

            // Disposes of the Event Processor Host
            await eventProcessorHost.UnregisterEventProcessorAsync();
        }
    }
}

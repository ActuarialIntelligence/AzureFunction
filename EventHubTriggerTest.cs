using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;


namespace test.Function
{
    public static class EventHubTriggerTest
    {
        [FunctionName("EventHubTriggerTest")]
        public static async Task Run([EventHubTrigger("hubtest", Connection = "testeventhubpredictive_RootManageSharedAccessKey_EVENTHUB")] EventData[] events, ILogger log)
        {
            var exceptions = new List<Exception>();
            CloudStorageAccount acc = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=name;AccountKey=Key;EndpointSuffix=core.windows.net");
            CloudBlobClient client = acc.CreateCloudBlobClient();
            CloudBlobContainer container = client.GetContainerReference("bloblubeck");
            //await container.CreateIfNotExistsAsync();

            Console.WriteLine("Successfully created Blob container");

            foreach (EventData eventData in events)
            {
                Console.WriteLine("Something is Happening!!");
                try
                {
                    string messageBody = Encoding.UTF8.GetString(eventData.Body.Array, eventData.Body.Offset, eventData.Body.Count);
                    InitialState sensorData  = JsonConvert.DeserializeObject<InitialState>(messageBody);
                    // Replace these two lines with your processing logic.
                    log.LogInformation($"C# Event Hub trigger function processed a message: {messageBody}");
                    Console.WriteLine(sensorData.humidity.ToString());
                    await Task.Yield();
                }
                catch (Exception e)
                {
                    // We need to keep processing the rest of the batch - capture this exception and continue.
                    // Also, consider capturing details of the message that failed processing so it can be processed again later.
                    exceptions.Add(e);
                }
            }

            // Once processing of the batch is complete, if any messages in the batch failed processing throw an exception so that there is a record of the failure.

            if (exceptions.Count > 1)
                throw new AggregateException(exceptions);

            if (exceptions.Count == 1)
                throw exceptions.Single();
        }
    }

    
    public class ThermalSensor
    {
        public IList<InitialState> initialState;
        public IList<Scripts> scripts;
        public DateTime interval;
    }

    public class InitialState
    {
        public int online;
        public double temperature;
        public int temperature_unit;
        public double humidity;
        public int humidity_unit;
        public double pressure;
        public int pressure_unit;
        public int simulation_state;
    }

    public class Scripts
    {
        public string Type;
        public string Path;
    }
}

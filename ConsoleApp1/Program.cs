using He.Iota.Net.Client;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var httpClient = new HttpClient();
            var tangleClient = new TangleClient(httpClient)
            {
                //BaseUrl = "http://192.168.1.101:14265"
                BaseUrl = "https://api.hornet-1.testnet.chrysalis2.com"
            };

            await tangleClient.HealthAsync();
            var info = await tangleClient.InfoAsync();

            Console.WriteLine(JsonConvert.SerializeObject(info, Formatting.Indented));


            var stopwatch = Stopwatch.StartNew();
            var submitMessageRequest = new SubmitMessageRequest();

            var indexationPayload = new IndexationPayload
            {
                Type = 2,
                Index = "TestIndex",
                Data = "Hello Wörld!"
            };

            submitMessageRequest.Payload = indexationPayload;
            var result = await tangleClient.SubmitMessage(submitMessageRequest);

            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));

            //list.Add(tangleClient.SubmitMessage(submitMessageRequest));

            //Console.WriteLine($"Added message {i}:");
            //Console.WriteLine(JsonConvert.SerializeObject(submitMessageResponse, Formatting.Indented));
            //}

            //await Task.WhenAll(list);

            //foreach(var item in list)
            //{
            //    Console.WriteLine(JsonConvert.SerializeObject(item.Result, Formatting.Indented));
            //}

            stopwatch.Stop();
            Console.WriteLine($"Elapsed Time in seconds: {stopwatch.ElapsedMilliseconds / 1000}");

            //// get message data
            //var messageResponse = await tangleClient.Messages3Async(submitMessageResponse.Data.MessageId);

            //Console.WriteLine("Message data:");
            //Console.WriteLine(JsonConvert.SerializeObject(messageResponse, Formatting.Indented));

            //// search by index
            //var messageFindResponse = await tangleClient.Messages2Async(submitMessageResponse.Data.MessageId);

            //Console.WriteLine("Found messages by index:");
            //Console.WriteLine(JsonConvert.SerializeObject(messageFindResponse, Formatting.Indented));
        }
    }
}

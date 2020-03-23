using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using MQTTnet.Formatter;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Connect_With_New_Mqtt_Features().Wait();
        }


        public static async Task Connect_With_New_Mqtt_Features()
        {
            Console.WriteLine("GG");
            var factory = new MqttFactory();
            // This test can be also executed against "broker.hivemq.com" to validate package format.
            var client = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                        .WithTcpServer("127.0.0.1", 1883)
                        .WithProtocolVersion(MqttProtocolVersion.V500)
                        .WithTopicAliasMaximum(20)
                        .WithReceiveMaximum(20)
                        .WithWillDelayInterval(20)
                        .Build();
            await client.ConnectAsync(options);

            MqttApplicationMessage receivedMessage = null;

            await client.SubscribeAsync("let/me/in/pep");

           
            Console.WriteLine("GG");
            while (true)
            {
                await client.PublishAsync(new MqttApplicationMessage() { Topic = "let/me/in/pep", Payload = Encoding.ASCII.GetBytes("mysecretdata") });

                client.UseApplicationMessageReceivedHandler(context => { receivedMessage = context.ApplicationMessage; Debug.WriteLine(receivedMessage.Payload.Length); });
                await Task.Delay(500);
            }
        }
    }
}

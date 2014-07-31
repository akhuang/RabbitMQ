using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Threading;

namespace Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var durable = true;
                    channel.QueueDeclare("task_queue", durable, false, false, null);
                    channel.BasicQos(0, 1, false);

                    var consumer = new QueueingBasicConsumer(channel);
                    channel.BasicConsume("task_queue", false, consumer);
           

                    Console.WriteLine(" [*] Waiting for messages." +
                                             "To exit press CTRL+C");
                    while (true)
                    {
                        var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();

                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine(" [x] Received {0}", message);

                        int dots = message.Split(',').Length - 1;
                        Console.WriteLine(dots);
                        Thread.Sleep(dots * 10 * 1000);

                        Console.WriteLine(" [x] Done");

                        channel.BasicAck(ea.DeliveryTag, false);
                    }
                }
            }
        }
    }
}

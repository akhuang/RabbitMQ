using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client;

namespace WorkQueues
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
                    //由于HelloWorld已经生成了一个hello的队列，所以这里再次生成且参数不一样会导致异常，RabbitMQ不允许这样操作
                    bool durable = true;
                    //channel.QueueDeclare("hello", durable, false, false, null);
                    channel.QueueDeclare("task_queue", durable, false, false, null);

                    string message = GetMessage(args);
                    var body = Encoding.UTF8.GetBytes(message);

                    var properties = channel.CreateBasicProperties();
                    properties.SetPersistent(true); //持久化，

                    channel.BasicPublish("", "task_queue", null, body);

                    Console.WriteLine(" [x] Sent {0}", message);
                }
            }

            Console.ReadKey();
        }

        private static string GetMessage(string[] args)
        {
            return (args.Length > 0) ? string.Join(" ", args) : "Hello world";
        }
    }
}

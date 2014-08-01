WorkQueues要点
1. 默认情况下队列发送消息给worker后会立即从队列中删除, 但此种情况下, 无法保证消息被worker顺利接收并执行;
   解决办法: 
    //第二个参数为:noAsk, 置为false表示需要worker回应Queue是否已经处理完消息
    channel.BasicConsume("task_queue", false, consumer);

    //执行完任务后调用下面代码告诉Queue消息已处理完毕
    channel.BasicAck(ea.DeliveryTag, false);
2. 
    //标识quque是持久性的
    bool durable = true;
    channel.QueueDeclare("task_queue", durable, false, false, null);
    //
    var properties = channel.CreateBasicProperties(); 
    properties.SetPersistent(true); //持久化，
2. 

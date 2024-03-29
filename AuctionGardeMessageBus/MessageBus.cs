﻿using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AuctionGardeMessageBus
{
    public class MessageBus : IMessageBus
    {
        private readonly string connectionString = "Endpoint=sb://auctiongardebus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=yl8CPFY6Vm0IQ/pAGghMR5z2Ynghg79aB+ASbPvj3h0=";
        public async Task PublishMessage(object message, string Topic_Queue_Name) //WILL ENSURE MESSAGE GETS TO QUEUE
        {
            //create a client to help us communicate with service bus
            var client = new ServiceBusClient(connectionString);
            ServiceBusSender sender = client.CreateSender(Topic_Queue_Name);   // takes up queue or topic name to know what queue its sending message to
            //prepare message
            //convert message to json
            var body = JsonConvert.SerializeObject(message);
            ServiceBusMessage theMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(body))  //the actual message to be sent
            {
                CorrelationId = Guid.NewGuid().ToString(),// ensures each message has a unique ID
            };

            //send message
            await sender.SendMessageAsync(theMessage);

            //free from resources
            await sender.DisposeAsync();

        }
    }
}

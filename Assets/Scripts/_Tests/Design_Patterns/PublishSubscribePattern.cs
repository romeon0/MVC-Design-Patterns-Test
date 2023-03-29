using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace UnityGame.Tests.DesignPatterns
{
    class PublishSubscribePattern
    {
        private interface IMessage
        {
        }

        private interface IPublisher<T> where T: IMessage
        {
            public void Publish(T message);
        }

        private interface IBroker<T> where T : IMessage
        {
            public T FetchMessage();
            public void AddMessage(T message);
        }

        private class Message : IMessage
        {
            public string publisherId;
            public string messageId;
            public DateTime timestamp;
        }

        private class Publisher : IPublisher<Message>
        {
            private IBroker<Message> _broker;
            private string _id;

            public string Id => _id;

            public Publisher(string id, IBroker<Message> broker)
            {
                _id = id;
                _broker = broker;
            }

            public void Publish(Message message)
            {
                _broker.AddMessage(message);
            }
        }

        private class Broker : IBroker<Message>
        {
            private List<Message> _messages = new List<Message>();

            public void AddMessage(Message message)
            {
                _messages.Add(message);
            }

            public Message FetchMessage()
            {
                if(_messages.Count == 0)
                {
                    return null;
                }

                Message message = _messages[0];
                
                _messages.RemoveAt(0);

                return message;
            }
        }

        private class MessageSender
        {
            private Publisher _publisher;

            public MessageSender(Publisher publisher)
            {
                _publisher = publisher;
            }

            public void Run()
            {
                SendMessagesRunner();
            }

            /// <summary>
            /// Server Side. The server generates and publish messages. Broker will receive the messages and will send to Consumers.
            /// </summary>
            private async void SendMessagesRunner()
            {
                int messageId = 0;
                while (Application.isPlaying)
                {
                    int count = UnityEngine.Random.Range(1, 3);

                    for (int a = 0; a < count; ++a)
                    {
                        Message message = new Message()
                        {
                            publisherId = _publisher.Id,
                            messageId = $"{messageId}",
                            timestamp = DateTime.Now

                        };

                        Debug.Log($"[PublishSubscribe][Sender] Sent message. PublisherId:{_publisher.Id}; MessageId: {messageId}");
                        _publisher.Publish(message);

                        ++messageId;
                    }

                    await Task.Delay(UnityEngine.Random.Range(1000, 2000));
                }
            }
        }

        private class MessageReceiver
        {
            private Broker _broker;

            public MessageReceiver(Broker broker)
            {
                _broker = broker;
            }

            public void Run()
            {
                ReceiveMessagesRunner();
            }

            /// <summary>
            /// Client Side. The client fetch messages from broker and process it.
            /// </summary>
            private async void ReceiveMessagesRunner()
            {
                while (Application.isPlaying)
                {
                    Message message;
                    do
                    {
                        message = _broker.FetchMessage();
                        if (message != null)
                        {
                            Debug.Log($"[PublishSubscribe][Consumer] Received message. PublishedId:{message.publisherId}; MessageId: {message.messageId}");
                        }
                    } while (message != null);

                    await Task.Delay(3000);
                }
            }
        }

        public class Tester
        {
            public void Test()
            {
                Broker broker = new Broker();
                Publisher publisher = new Publisher("PUB_1", broker);

                MessageSender sender = new MessageSender(publisher);
                sender.Run();

                MessageReceiver receiver = new MessageReceiver(broker);
                receiver.Run();
            }
        }
    }
}

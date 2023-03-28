using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityGame.Tests.DesignPatterns
{
    class ObserverPattern
    {
        private interface IMessage
        {
        }

        private interface ISubject<T> where T: IMessage
        {
            public void Subscribe(IObserver<T> action);
            public void Publish(T message);
        }

        private interface IObserver<T> where T: IMessage
        {
            public void Handle(T message);
        }

        private class Message : IMessage
        {
            public string id;
        }

        private class Subject : ISubject<Message>
        {
            private List<IObserver<Message>> _subscribers = new List<IObserver<Message>>();

            public void Subscribe(IObserver<Message> observer)
            {
                _subscribers.Add(observer);
            }

            public void Publish(Message message)
            {
                foreach(var subscriber in _subscribers)
                {
                    subscriber.Handle(message);
                }
            }
        }

        private class Observer : IObserver<Message>
        {
            private int _id;
            public Observer(int id)
            {
                _id = id;
            }

            public void Handle(Message message)
            {
                Debug.Log($"[rrr] Received message. ObserverId:{_id}; MessageId: {message.id}");
            }
        }

        public class Tester
        {
            public void Test()
            {
                Message message = new Message() { id = "hello_1" };
                Message message2 = new Message() { id = "hello_2" };

                Observer observer = new Observer(1);
                Observer observer2 = new Observer(2);
                Observer observer3 = new Observer(3);

                Subject subject = new Subject();

                subject.Subscribe(observer);
                subject.Subscribe(observer2);
                subject.Subscribe(observer3);

                subject.Publish(message);
                subject.Publish(message2);
            }
        }
    }
}

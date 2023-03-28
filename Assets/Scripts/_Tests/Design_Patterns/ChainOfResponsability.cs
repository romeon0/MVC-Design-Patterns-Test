using UnityEngine;

namespace UnityGame.Tests.DesignPatterns
{
    class ChainOfResponsability
    {
        private interface IHandler
        {
            public bool Handle(IMessage message);
           // public void MoveNext();
        }

        private class Handler : IHandler
        {
            protected Handler _next;

            public Handler(Handler next)
            {
                _next = next;
            }

            public bool Handle(IMessage message)
            {
                if (HandleInternal(message))
                {
                    return true;
                }

                if(_next == null)
                {
                    return false;
                }

                return _next.Handle(message);
            }

            protected virtual bool HandleInternal(IMessage message)
            {
                return true;
            }

            //public void MoveNext()
            //{
            //    _next.Handle(_message);
            //}
        }

        private class LoginHandler : Handler
        {
            public LoginHandler(Handler next) : base(next) { }
            protected override bool HandleInternal(IMessage message)
            {
                if(message is LoginMessage)
                {
                    Debug.Log($"[Chain] Message handled. MsgType:{message.GetType().Name}; HandlerType:{GetType().Name}");
                    return true;
                }
                return false;
            }
        }

        private class RegisterHandler : Handler
        {
            public RegisterHandler(Handler next) : base(next) { }
            protected override bool HandleInternal(IMessage message)
            {
                if (message is RegisterMessage)
                {
                    Debug.Log($"[Chain] Message handled. MsgType:{message.GetType().Name}; HandlerType:{GetType().Name}");
                    return true;
                }
                return false;
            }
        }

        private class MainHandler : Handler
        {
            public MainHandler(Handler next) : base(next) { }
            protected override bool HandleInternal(IMessage message)
            {
                if (_next.Handle(message))
                {
                    return true;
                }

                Debug.Log($"[Chain] Message handled by Default Handler. " +
                    $"MsgType:{message.GetType().Name}; HandlerType:{GetType().Name}");
                return false;
            }
        }



        private interface IMessage
        {

        }

        private class LoginMessage : IMessage
        {
            public string userId;
        }

        private class RegisterMessage : IMessage
        {
            public string userId;
        }

        private class GetInventoryMessage : IMessage
        {
            public string userId;
        }

        public class Tester
        {
            private MainHandler _mainHandler;

            public void Test()
            {
                LoginMessage message = new LoginMessage();
                RegisterMessage message2 = new RegisterMessage();
                GetInventoryMessage message3 = new GetInventoryMessage();

                LoginHandler handler3 = new LoginHandler(null);
                RegisterHandler handler2 = new RegisterHandler(handler3);
                _mainHandler = new MainHandler(handler2);

                SendMessage(message);
                SendMessage(message2);
                SendMessage(message3);
            }

            private void SendMessage(IMessage message)
            {
                _mainHandler.Handle(message);
            }
        }
    }
}

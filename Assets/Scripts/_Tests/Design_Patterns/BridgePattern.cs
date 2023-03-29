using UnityEngine;

namespace UnityGame.Tests.DesignPatterns
{
    class BridgePattern
    {
        private interface IOperationSystem
        {
            public void RunApp(string identifier);
        }

        private class AndroidOS : IOperationSystem
        {
            public void RunApp(string identifier) { Debug.Log("[Bridge] RunApp called for iOS"); }
        }

        private class iOS : IOperationSystem
        {
            public void RunApp(string identifier) { Debug.Log("[Bridge] RunApp called for Android"); }
        }

        private class OperationSystemController
        {
            private IOperationSystem _os;
            public OperationSystemController(IOperationSystem os)
            {
                _os = os;
            }

            public void RunApp(string identifier)
            {
                _os.RunApp(identifier);
            }
        }

        public class Tester
        {
            private IOperationSystem _android = new AndroidOS();
            private IOperationSystem _ios = new iOS();

            public void Test()
            {
                string appId = "com.company.hello";

                IOperationSystem os;
                bool isAndroid = true;
                if (isAndroid)
                {
                    os = _android;
                }
                else
                {
                    os = _ios;
                }

                OperationSystemController controller = new OperationSystemController(os);
                controller.RunApp(appId);
            }
        }
    }
}

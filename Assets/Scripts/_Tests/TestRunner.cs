using UnityGame.Tests.DesignPatterns;

namespace UnityGame.Tests
{
    public class TestRunner
    {
        public void Run()
        {
            var observer = new ObserverPattern.Tester();
            observer.Test();

            var publishSubscribe = new PublishSubscribePattern.Tester();
            publishSubscribe.Test();

            var chainOfResponsability = new ChainOfResponsabilityPattern.Tester();
            chainOfResponsability.Test();

            var bridge = new BridgePattern.Tester();
            bridge.Test();

            var memento = new MementoPattern.Tester();
            memento.Test();

            var iterator = new IteratorPattern.Tester();
            iterator.Test();
        }
    }
}

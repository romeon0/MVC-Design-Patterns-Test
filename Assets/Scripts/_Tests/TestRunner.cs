using UnityGame.Tests.DesignPatterns;

namespace UnityGame.Tests
{
    public class TestRunner
    {
        public void Run()
        {
            //var observerTest = new ObserverPattern.Tester();
            //observerTest.Test();

            //var publishSubscribeTest = new PublishSubscribe.Tester();
            //publishSubscribeTest.Test();

            //var chainOfResponsabilityTest = new ChainOfResponsability.Tester();
            //chainOfResponsabilityTest.Test();

            var memento = new Memento.Tester();
            memento.Test();

            var iterator = new IteratorDesignPattern.Tester();
            iterator.Test();
        }
    }
}

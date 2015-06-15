using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cyperus;

namespace Cyperus.Tests
{
    [TestClass]
    public class BasicTest
    {
        [TestMethod]
        public void TestDataTransfer()
        {
            var prod = new TestProducer("prod", null);
            var cons = new TestConsumer("cons", null);

            prod.Outputs[0].ConnectTo(cons.Inputs[0]);

            prod.Count = 20;
            prod.Start();

            while (!prod.Finished)
            {
                Thread.Sleep(500);
            }

            Thread.Sleep(100);
            
            Assert.AreEqual(210, cons.Buffer);
        }

        [TestMethod]
        public void TestDataProcess()
        {
            var prod = new TestProducer("prod", null);
            var cons = new TestConsumer("cons", null);
            var proc = new TestProcessor("proc", null);

            prod.Outputs[0].ConnectTo(proc.Inputs[0]);
            proc.Outputs[0].ConnectTo(cons.Inputs[0]);

            prod.Count = 20;
            prod.Start();

            while (!prod.Finished)
            {
                Thread.Sleep(500);
            }

            Thread.Sleep(100);

            Assert.AreEqual(420, cons.Buffer);
        }
    }
}

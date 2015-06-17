using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Cyperus;

namespace Cyperus.Tests
{
    class TestProducer : Producer
    {
        Socket<int> Out;
        
        public int Count;
        public bool Finished = false;
        
        public TestProducer(string name, Environment env)
            : base(name, env)
        {
            Out = AddOutput<int>("Numbers");
            Console.WriteLine("Created producer");
        }

        protected override void Produce()
        {
            Console.WriteLine("Start producing");

            try
            {
                for (int i = 1; i <= Count; i++)
                {
                    Console.WriteLine("Producing...");

                    if (FPaused)
                    {
                        try
                        {
                            Thread.Sleep(Timeout.Infinite);
                        }
                        catch (ThreadInterruptedException e)
                        {
                            
                        }
                    }

                    Outputs.ElementAt(0).AcceptData(this, i);
                    Console.WriteLine(String.Format("Out: {0}", i));
                    Thread.Sleep(100);
                }

                Finished = true;
            }
            catch (ThreadAbortException e)
            {
                Finished = true;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cyperus;

namespace Virtual16.Nodes
{
    public class ClockGenerator : Producer
    {
        public int Interval = 100;
        
        protected readonly Socket<object> Output;
        
        public ClockGenerator(string name, Cyperus.Environment env)
            : base(name, env)
        {
            Output = AddOutput<object>("out");
        }
        
        protected override void Produce()
        {
            try
            {
                while (true)
                {
                    try
                    {
                        while (FPaused)
                            Thread.Sleep(1000);

                        Thread.Sleep(Interval);

                        if (OnSocketActivity != null)
                            OnSocketActivity(this, Output);

                        Output.AcceptData(this, 0);
                    }
                    catch (ThreadInterruptedException e)
                    {
                        continue;
                    }
                }
            }
            catch (ThreadAbortException e)
            {
                return;
            }
        }

        public override async Task AcceptData(ISender sender, object data)
        {
            
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cyperus
{
    public abstract class Producer : AbstractNode, ISender
    {
        protected Thread Thread;
        protected bool FPaused = false;

        public bool Paused
        {
            get { return FPaused; }
            set
            {
                if (!value && FPaused)
                {
                    Thread.Interrupt();
                }

                FPaused = value;
            }
        }

        protected Producer(string name)
            : base(name)
        {
            Thread = new Thread(DoProduce);
        }

        public void Start()
        {
            if (Thread == null)
            {
                Thread = new Thread(DoProduce);
            }

            Thread.Start();
        }

        public void Stop()
        {
            Thread.Abort();
            Thread = null;
        }

        protected abstract void DoProduce();

        ~Producer()
        {
            Thread.Abort();
        }
    }
}

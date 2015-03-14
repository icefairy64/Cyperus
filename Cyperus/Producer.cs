using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cyperus
{
    [Serializable]
    public abstract class Producer : AbstractNode, ISender
    {
        [NonSerialized]
        protected Thread Thread;
        [NonSerialized]
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

        protected Producer(string name, Environment env)
            : base(name, env)
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

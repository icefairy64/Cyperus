using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Cyperus
{
    [JsonObject(MemberSerialization.OptIn)]
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

        /// <summary>
        /// Produces data and sends it to clients. Requires to handle typical thread messages (interruption, abortion)
        /// </summary>
        protected abstract void DoProduce();

        ~Producer()
        {
            Thread.Abort();
        }
    }
}

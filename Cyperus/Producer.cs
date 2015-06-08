using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Cyperus
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn, IsReference = true)]
    public abstract class Producer : AbstractNode, ISender
    {
        protected Thread ProcThread;
        protected bool FPaused = false;

        public bool Paused
        {
            get { return FPaused; }
            set
            {
                if (!value && FPaused)
                {
                    ProcThread.Interrupt();
                }

                FPaused = value;
            }
        }

        protected Producer(string name, Environment env)
            : base(name, env)
        {
            ProcThread = new Thread(Produce);
        }

        public void Start()
        {
            if (ProcThread == null)
            {
                ProcThread = new Thread(Produce);
            }

            ProcThread.Start();
        }

        public void Stop()
        {
            if (ProcThread != null)
            {
                ProcThread.Abort();
                ProcThread = null;
            }
        }

        /// <summary>
        /// Produces data and sends it to clients. This method should handle typical thread messages (interruption, abortion)
        /// </summary>
        protected abstract void Produce();

        ~Producer()
        {
            if (ProcThread != null)
            {
                ProcThread.Abort();
            }
        }
    }
}

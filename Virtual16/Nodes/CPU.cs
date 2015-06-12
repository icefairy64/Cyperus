using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cyperus;

namespace Virtual16.Nodes
{
    public class CPUInterrupt
    {
        public readonly V16CPU Sender;

        public CPUInterrupt(V16CPU sender)
        {
            Sender = sender;
        }
    }
    
    public class CPU : Processor
    {
        protected readonly Socket<object> Clock;
        protected readonly Socket<byte[]> Command;
        protected readonly Socket<CPUInterrupt> Reset;
        protected readonly Socket<CPUInterrupt> HWInterrupt;
        protected readonly V16CPU Core;
        
        public CPU(string name, Cyperus.Environment env)
            : base(name, env)
        {
            Clock = AddInput<object>("clock");
            Command = AddInput<byte[]>("cmd");
            Reset = AddOutput<CPUInterrupt>("reset");
            HWInterrupt = AddOutput<CPUInterrupt>("hw");
            Core = new V16CPU();

            Core.ResetHandler = ResetHandler;
            Core.ExternalHWMemUpdateHandler = HWInterruptHandler;
        }

        protected void ResetHandler(V16CPU sender)
        {
            SendToSocket(Reset, new CPUInterrupt(sender));
        }

        protected void HWInterruptHandler(V16CPU sender)
        {
            SendToSocket(HWInterrupt, new CPUInterrupt(sender));
        }

        protected override async Task<object> ProcessData(ISender sender, object data)
        {
            return await Task.FromResult(0);
        }

        protected override async Task DispatchData(ISender sender, object data)
        {
            await Task.FromResult(0);
        }

        public override async Task AcceptData(ISender sender, object data)
        {
            if (OnSocketActivity != null)
                OnSocketActivity(this, sender);

            var cmd = data as byte[];
            if (cmd == null)
                await Task.Run(() => Core.Tick());
            else
                await Task.Run(() => Core.Exec(cmd));
        }
    }
}

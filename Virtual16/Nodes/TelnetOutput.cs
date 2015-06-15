using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using Cyperus;

namespace Virtual16.Nodes
{
    public class TelnetOutput : Output
    {
        protected readonly Socket<TelnetClient> ConnectionReceiver;
        protected readonly Socket<CPUInterrupt> InterruptInput;
        protected NetworkStream Stream = null;

        public TelnetOutput(string name, Cyperus.Environment env)
            : base(name, env)
        {
            ConnectionReceiver = AddInput<TelnetClient>("connection");
            InterruptInput = AddInput<CPUInterrupt>("interrupt");
        }

        public override async Task AcceptData(ISender sender, object data)
        {
            await base.AcceptData(sender, data);

            if (data is TelnetClient)
            {
                var client = data as TelnetClient;
                Stream = client.Stream;
            }
            else if (data is CPUInterrupt)
            {
                var ip = data as CPUInterrupt;

                ushort i = V16CPU.SharedMemOffset;
                char c = '\0';
                string buf = "";
                while (i < V16CPU.HWStatusOffset && (c = (char)Memory.LoadByte(ip.Sender.Memory, i)) != '\0')
                {
                    buf += c;
                    i++;
                }

                Stream.Write(TelnetServer.StringToByteArray(buf), 0, buf.Length);
            }
        }
    }
}

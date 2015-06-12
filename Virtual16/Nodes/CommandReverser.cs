using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cyperus;

namespace Virtual16.Nodes
{
    public class CommandReverser : Processor
    {
        protected Socket<TelnetCommand> In;
        protected Socket<TelnetCommand> Out;

        public CommandReverser(string name, Cyperus.Environment env)
            : base(name, env)
        {
            In = AddInput<TelnetCommand>("in");
            Out = AddOutput<TelnetCommand>("out");
        }

        protected override Task DispatchData(ISender sender, object data)
        {
            throw new NotImplementedException();
        }

        protected override Task<object> ProcessData(ISender sender, object data)
        {
            throw new NotImplementedException();
        }
    }
}

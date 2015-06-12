using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cyperus;
using Virtual16.Nodes.Forms;

namespace Virtual16.Nodes
{
    public class CommandListener : Output
    {
        protected readonly Socket<TelnetCommand> In;

        [PropertiesForm]
        public CommandListenerForm Form;

        public CommandListener(string name, Cyperus.Environment env)
            : base(name, env)
        {
            In = AddInput<TelnetCommand>("in");
        }

        public override async Task AcceptData(ISender sender, object data)
        {
            var cmd = data as TelnetCommand;
            if (data == null)
                return;

            var query =
                from x in cmd.Value.ToCharArray()
                select String.Format("{0:X2} ", (byte)x);

            var hex = query.Aggregate("", (a, x) => { return a + x; });

            if (Form != null)
                Form.AddLine(String.Format("Received CMD: {0} ({1} bytes)\nString: {2}\n", hex, cmd.Value.Length, cmd.Value));

            if (OnSocketActivity != null)
                OnSocketActivity(this, In);
        }
    }
}

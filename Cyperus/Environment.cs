using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyperus
{
    public class Environment
    {
        public List<AbstractNode> Nodes { get; protected set; }
        public ImmutableList<Connection> Connections { get; protected set; }

        public Environment()
        {
            Nodes = new List<AbstractNode>();
            Connections = ImmutableList.Create<Connection>();
        }

        public void Connect(AbstractSocket socket1, AbstractSocket socket2)
        {
            if (socket1 == socket2)
            {
                return;
            }
            
            if (Connections.FindIndex(c => ((c.Client == socket1) || (c.Client == socket2)) && ((c.Server == socket1) || (c.Server == socket2))) >= 0)
            {
                return;
            }
            
            var conn = socket1.ConnectTo(socket2);
            Connections = Connections.Add(conn);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyperus
{
    [Serializable]
    public class Connection : IEquatable<Connection>
    {
        public AbstractSocket Server { get; protected set; }
        public AbstractSocket Client { get; protected set; }

        public Connection(AbstractSocket server, AbstractSocket client)
        {
            Server = server;
            Client = client;
        }
        
        public void Destroy(bool disconnectSockets = true)
        {
            Server.RemoveClient(Client);
            if (disconnectSockets)
            {
                Server.Disconnect(this);
                Client.Disconnect(this);
            }
        }

        public bool Equals(Connection obj)
        {
            return (obj.Server == Server) && (obj.Client == Client);
        }
    }
}

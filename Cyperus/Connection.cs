using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyperus
{
    public class Connection : IEquatable<Connection>
    {
        public AbstractSocket Server;
        public AbstractSocket Client;

        public Connection(AbstractSocket server, AbstractSocket client)
        {
            Server = server;
            Client = client;
        }
        
        public void Destroy()
        {
            Server.RemoveClient(Client);
        }

        public bool Equals(Connection obj)
        {
            return (obj.Server == Server) && (obj.Client == Client);
        }
    }
}

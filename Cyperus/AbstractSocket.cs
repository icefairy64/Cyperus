using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyperus
{
    /// <summary>
    /// Represents a socket that can send and accept untyped data
    /// </summary>
    [Serializable]
    abstract public class AbstractSocket : IAcceptor, ISender
    {
        public Type DataType { get; protected set; }
        public string Name { get; protected set; }

        protected IAcceptor Owner;
        protected ImmutableList<AbstractSocket> Clients;
        protected Dictionary<AbstractSocket, Connection> Connections;

        protected AbstractSocket(IAcceptor acceptor, string name, Type dataType)
        {
            Owner = acceptor;
            Name = name;
            DataType = dataType;

            Clients = ImmutableList.Create<AbstractSocket>();
            Connections = new Dictionary<AbstractSocket, Connection>();
        }

        public void AddClient(AbstractSocket client)
        {
            if (!client.DataType.IsSubclassOf(DataType))
            {
                throw new TypeMismatchException(String.Format("Types of sockets {0} and {1} don't match", this, client));
            }

            if (Clients.Contains(client))
            {
                return;
            }

            Clients = Clients.Add(client);
        }

        public void RemoveClient(AbstractSocket client)
        {
            if (!Clients.Contains(client))
            {
                return;
            }
            
            Clients = Clients.Remove(client);
        }

        public async Task AcceptData(ISender sender, Object data)
        {
            if (!data.GetType().IsSubclassOf(DataType))
            {
                throw new TypeMismatchException(String.Format("Socket {0} doesn't accept data of type {1}", this, data.GetType()));
            }
            
            if (Owner == null)
                return;
            else if (sender == Owner)
                await SendData(data);
            else
                await Owner.AcceptData(this, data);
        }

        protected async Task SendData(Object data)
        {
            if (!data.GetType().IsSubclassOf(DataType))
            {
                throw new TypeMismatchException(String.Format("Socket {0} doesn't send data of type {1}", this, data.GetType()));
            }

            var query =
                from acceptor in Clients select acceptor.AcceptData(this, data);
            
            await Task.WhenAll(query.ToArray<Task>());
        }

        public Connection ConnectTo(AbstractSocket socket)
        {
            if (socket == this)
            {
                return null;
            }
            
            if (Owner == null)
            {
                // This is client socket; we should add it to another socket's client list
                return socket.ConnectTo(this);
            }

            if (Clients.Contains(socket))
            {
                return Connections[socket];
            }

            var conn = new Connection(this, socket);
            AddClient(socket);
            Connections.Add(socket, conn);
            socket.AcceptConnection(this, conn);

            return conn;
        }

        public void AcceptConnection(AbstractSocket sender, Connection conn)
        {
            if (!Connections.ContainsKey(sender))
            {
                Connections.Add(sender, conn);
            }
        }

        public void Destroy()
        {
            foreach (var conn in Connections.Values)
            {
                conn.Destroy();
            }

            Connections.Clear();
        }

        public override string ToString()
        {
            return String.Format("{0} ({1})", Name, DataType);
        }

        ~AbstractSocket()
        {
            Destroy();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Cyperus
{
    public enum SocketKind
    {
        Source,
        Destination
    }
    
    /// <summary>
    /// Represents a socket that can send and accept untyped data
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn, IsReference = true)]
    abstract public class AbstractSocket : IAcceptor, ISender
    {
        [JsonProperty]
        public Type DataType { get; protected set; }
        [JsonProperty]
        public string Name { get; protected set; }
        [JsonProperty]
        public readonly SocketKind Kind;

        public IReadOnlyList<AbstractSocket> Clients
        {
            get { return FClients; }
        }

        [JsonProperty]
        protected IAcceptor Owner;
        [JsonProperty]
        protected Dictionary<AbstractSocket, Connection> Connections;

        [JsonProperty]
        protected List<AbstractSocket> FClients;

        protected AbstractSocket(IAcceptor acceptor, string name, Type dataType, SocketKind kind)
        {
            Owner = acceptor;
            Name = name;
            DataType = dataType;
            Kind = kind;

            FClients = new List<AbstractSocket>();
            Connections = new Dictionary<AbstractSocket, Connection>();
        }

        public void AddClient(AbstractSocket client)
        {
            if (!AcceptsDataType(client.DataType))
            {
                throw new TypeMismatchException(String.Format("Types of sockets {0} and {1} don't match", this, client));
            }

            if (FClients.Contains(client))
            {
                return;
            }

            FClients.Add(client);
        }

        public void RemoveClient(AbstractSocket client)
        {
            if (!FClients.Contains(client))
            {
                return;
            }
            
            FClients.Remove(client);
        }

        public bool AcceptsDataType(Type type)
        {
            return type.IsSubclassOf(DataType) || type == DataType;
        }

        public async Task AcceptData(ISender sender, Object data)
        {
            if (!AcceptsDataType(data.GetType()))
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
            if (!AcceptsDataType(data.GetType()))
            {
                throw new TypeMismatchException(String.Format("Socket {0} doesn't send data of type {1}", this, data.GetType()));
            }

            var query =
                from acceptor in FClients select acceptor.AcceptData(this, data);
            
            await Task.WhenAll(query.ToArray<Task>());
        }

        public Connection ConnectTo(AbstractSocket socket)
        {
            if (socket == this)
            {
                return null;
            }
            
            if (Kind == SocketKind.Destination)
            {
                // This is client socket; we should add it to another socket's client list
                return socket.ConnectTo(this);
            }

            if (FClients.Contains(socket))
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

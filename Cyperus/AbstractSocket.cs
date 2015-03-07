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
    abstract public class AbstractSocket : IAcceptor, ISender
    {
        public Type DataType { get; protected set; }
        public ImmutableList<AbstractSocket> Clients { get; protected set; }
        public string Name { get; protected set; }

        protected IAcceptor Owner;

        protected AbstractSocket(IAcceptor acceptor, string name, Type dataType)
        {
            Owner = acceptor;
            Name = name;
            DataType = dataType;

            Clients = ImmutableList.Create<AbstractSocket>();
        }

        public void AddClient(AbstractSocket client)
        {
            if (!client.DataType.IsSubclassOf(DataType))
            {
                throw new TypeMismatchException(String.Format("Types of sockets {0} and {1} don't match", this, client));
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
            
            await Owner.AcceptData(this, data);
        }

        public async Task SendData(IAcceptor acceptor, Object data)
        {
            if (!data.GetType().IsSubclassOf(DataType))
            {
                throw new TypeMismatchException(String.Format("Socket {0} doesn't send data of type {1}", this, data.GetType()));
            }
            
            await acceptor.AcceptData(this, data);
        }

        public async Task SendData(Object data)
        {
            if (!data.GetType().IsSubclassOf(DataType))
            {
                throw new TypeMismatchException(String.Format("Socket {0} doesn't send data of type {1}", this, data.GetType()));
            }
            
            var query = 
                from acceptor in Clients select SendData(acceptor, data);
            
            await Task.WhenAll(query.ToArray<Task>());
        }

        public override string ToString()
        {
            return String.Format("{0} ({1})", Name, DataType);
        }
    }
}

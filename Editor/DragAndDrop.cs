using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Cyperus.Designer
{
    internal struct NodeCreationContainer
    {
        public readonly Type NodeType;

        public NodeCreationContainer(Type nodeType)
        {
            NodeType = nodeType;
        }
    }

    internal struct ConnectionContainer
    {
        public readonly SocketWrapper Source;

        public ConnectionContainer(SocketWrapper src)
        {
            Source = src;
        }
    }

    public class SocketWrapper
    {
        public readonly NodeBox Owner;
        public readonly object Socket;
        public bool Flash;
        public Point Location;

        public IReadOnlyList<ConnectionWrapper> Connections
        {
            get { return FConnections; }
        }

        protected List<ConnectionWrapper> FConnections;

        public SocketWrapper(NodeBox owner, object socket, int x, int y)
        {
            Socket = socket;
            Flash = false;
            Location = new Point(x, y);
            Owner = owner;
            FConnections = new List<ConnectionWrapper>();
        }

        public void AddConnection(ConnectionWrapper conn)
        {
            if (FConnections.Contains(conn))
                return;

            FConnections.Add(conn);
        }

        public ConnectionWrapper PopConnection()
        {
            var conn = FConnections.Last();
            FConnections.Remove(conn);
            return conn;
        }

        public void RemoveConnection(ConnectionWrapper conn)
        {
            FConnections.Remove(conn);
        }
    }

    public class ConnectionWrapper
    {
        public readonly SocketWrapper Source;
        public readonly SocketWrapper Destination;
        public readonly Connection Connection;

        public ConnectionWrapper(SocketWrapper src, SocketWrapper dest, Connection conn)
        {
            Source = src;
            Destination = dest;
            Connection = conn;
        }
    }
}

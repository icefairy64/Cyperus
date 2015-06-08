using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Cyperus
{
    /// <summary>
    /// Represents a socket that can send and accept typed data
    /// </summary>
    /// <typeparam name="T">Type of data that socket is able to work with</typeparam>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn, IsReference = true)]
    public class Socket<T> : AbstractSocket
    {
        public Socket(IAcceptor acceptor, string name, SocketKind kind)
            : base(acceptor, name, typeof(T), kind)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyperus
{
    abstract public class Socket
    {
        public Type DataType { get; protected set; }
        public List<Socket> Clients { get; protected set; }
    }
}

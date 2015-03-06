using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyperus
{
    abstract public class Node
    {
        public List<Socket> Inputs { get; protected set; }
        public List<Socket> Outputs { get; protected set; }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyperus
{
    /// <summary>
    /// Represents a node that has several (or none) input and output sockets
    /// </summary>
    abstract public class AbstractNode
    {
        public ImmutableList<AbstractSocket> Inputs { get; protected set; }
        public ImmutableList<AbstractSocket> Outputs { get; protected set; }
        public string Name { get; set; }

        protected AbstractNode(string name)
        {
            Name = name;
            Inputs = ImmutableList.Create<AbstractSocket>();
            Outputs = ImmutableList.Create<AbstractSocket>();
        }
    }
}

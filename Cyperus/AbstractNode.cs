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
    [Serializable]
    abstract public class AbstractNode : IAcceptor
    {
        public ImmutableList<AbstractSocket> Inputs { get; protected set; }
        public ImmutableList<AbstractSocket> Outputs { get; protected set; }
        public string Name { get; set; }
        public Environment Environment { get; protected set; }

        protected AbstractNode(string name, Environment env)
        {
            Name = name;
            Inputs = ImmutableList.Create<AbstractSocket>();
            Outputs = ImmutableList.Create<AbstractSocket>();
            Environment = env;
        }

        protected void AddInput<T>(string name)
        {
            var socket = new Socket<T>(this, name);
            Inputs = Inputs.Add(socket);
        }

        protected void AddOutput<T>(string name)
        {
            var socket = new Socket<T>(null, name);
            Outputs = Outputs.Add(socket);
        }

        protected void RemoveInput(AbstractSocket socket)
        {
            if (Inputs.Contains(socket))
            {
                Inputs = Inputs.Remove(socket);
            }
        }

        protected void RemoveOutput(AbstractSocket socket)
        {
            if (Outputs.Contains(socket))
            {
                Outputs = Outputs.Remove(socket);
            }
        }

        public abstract Task AcceptData(ISender sender, Object data);
    }
}

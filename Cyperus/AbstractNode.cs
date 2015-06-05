using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Cyperus
{
    /// <summary>
    /// Represents a node that has several (or none) input and output sockets
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    abstract public class AbstractNode : IAcceptor
    {
        public IReadOnlyList<AbstractSocket> Inputs
        {
            get { return FInputs; }
        }

        public IReadOnlyList<AbstractSocket> Outputs 
        {
            get { return FOutputs; }
        }

        [JsonProperty]
        public string Name { get; set; }
        [JsonProperty]
        public Environment Environment { get; protected set; }

        [JsonProperty]
        protected List<AbstractSocket> FInputs;
        [JsonProperty]
        protected List<AbstractSocket> FOutputs;

        protected AbstractNode(string name, Environment env)
        {
            Name = name;
            FInputs = new List<AbstractSocket>();
            FOutputs = new List<AbstractSocket>();
            Environment = env;
        }

        protected void AddInput<T>(string name)
        {
            var socket = new Socket<T>(this, name);
            FInputs.Add(socket);
        }

        protected void AddOutput<T>(string name)
        {
            var socket = new Socket<T>(null, name);
            FOutputs.Add(socket);
        }

        protected void RemoveInput(AbstractSocket socket)
        {
            if (Inputs.Contains(socket))
            {
                FInputs.Remove(socket);
            }
        }

        protected void RemoveOutput(AbstractSocket socket)
        {
            if (Outputs.Contains(socket))
            {
                FOutputs.Remove(socket);
            }
        }

        public abstract Task AcceptData(ISender sender, Object data);
    }
}

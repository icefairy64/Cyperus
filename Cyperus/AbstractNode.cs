using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Cyperus
{
    public delegate void NodeUpdateHandler(AbstractNode sender);
    public delegate Task SocketActivityHandler(AbstractNode sender, object socket);
    
    /// <summary>
    /// Represents a node that has several (or none) input and output sockets
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn, IsReference = true)]
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

        public NodeUpdateHandler OnUpdate;
        public SocketActivityHandler OnSocketActivity;

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

        protected Socket<T> AddInput<T>(string name)
        {
            var socket = new Socket<T>(this, name);
            FInputs.Add(socket);

            if (OnUpdate != null)
                OnUpdate(this);

            return socket;
        }

        protected Socket<T> AddOutput<T>(string name)
        {
            var socket = new Socket<T>(null, name);
            FOutputs.Add(socket);

            if (OnUpdate != null)
                OnUpdate(this);

            return socket;
        }

        protected void RemoveInput(AbstractSocket socket)
        {
            if (Inputs.Contains(socket))
            {
                FInputs.Remove(socket);
            }

            if (OnUpdate != null)
                OnUpdate(this);
        }

        protected void RemoveOutput(AbstractSocket socket)
        {
            if (Outputs.Contains(socket))
            {
                FOutputs.Remove(socket);
            }

            if (OnUpdate != null)
                OnUpdate(this);
        }

        public void Destroy()
        {
            foreach (var socket in FInputs)
                socket.Destroy();

            foreach (var socket in FOutputs)
                socket.Destroy();
        }

        public async virtual Task AcceptData(ISender sender, Object data)
        {
            if (OnSocketActivity != null)
                await OnSocketActivity(this, sender);
        }
    }
}

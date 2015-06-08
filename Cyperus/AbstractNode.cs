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

        public bool Destroyed
        {
            get { return FDestroyed; }
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

        protected bool FDestroyed = false;

        protected AbstractNode(string name, Environment env)
        {
            Name = name;
            FInputs = new List<AbstractSocket>();
            FOutputs = new List<AbstractSocket>();
            Environment = env;

            if (env != null)
                env.Nodes.Add(this);
        }

        protected Socket<T> AddInput<T>(string name)
        {
            var socket = new Socket<T>(this, name, SocketKind.Destination);
            FInputs.Add(socket);

            if (OnUpdate != null)
                OnUpdate(this);

            return socket;
        }

        protected Socket<T> AddOutput<T>(string name)
        {
            var socket = new Socket<T>(this, name, SocketKind.Source);
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

        /// <summary>
        /// Destroys node and removes it from its environment
        /// </summary>
        public virtual void Destroy()
        {
            foreach (var socket in FInputs)
                socket.Destroy();

            foreach (var socket in FOutputs)
                socket.Destroy();

            if (Environment != null)
                Environment.Nodes.Remove(this);

            FDestroyed = true;
        }

        public async virtual Task AcceptData(ISender sender, Object data)
        {
            if (OnSocketActivity != null)
                OnSocketActivity(this, sender);

            // Hack: to call OnSocketActivity in background we need to call something with await instead
            // In this case, we're calling method that will return predefined result
            await Task.FromResult(0);
        }

        public override string ToString()
        {
            return String.Format("{0} ({1})", Name, GetType().FullName);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IO;

namespace Cyperus
{
    /// <summary>
    /// Represents an environment for node structures
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn, IsReference = true)]
    public class Environment
    {
        static JsonSerializer Serializer = new JsonSerializer();

        public IReadOnlyList<Connection> Connections
        {
            get { return FConnections; }
        }

        [JsonProperty]
        public List<AbstractNode> Nodes { get; protected set; }
        [JsonProperty]
        public Dictionary<string, Object> Storage { get; protected set; }

        [JsonProperty]
        protected List<Connection> FConnections;

        [JsonProperty]
        protected List<Assembly> Dependencies;

        public Environment()
        {
            Nodes = new List<AbstractNode>();
            FConnections = new List<Connection>();
            Storage = new Dictionary<string, Object>();
            Dependencies = new List<Assembly>();
        }

        public static Environment Load(string filename)
        {
            using (var reader = new StreamReader(filename))
            using (var json = new JsonTextReader(reader))
            {
                return (Environment)Serializer.Deserialize<Environment>(json);
            }
        }

        public void Connect(AbstractSocket socket1, AbstractSocket socket2)
        {
            if (socket1 == socket2)
                return;
            
            if (FConnections.FindIndex(c => ((c.Client == socket1) || (c.Client == socket2)) && ((c.Server == socket1) || (c.Server == socket2))) >= 0)
                return;
            
            var conn = socket1.ConnectTo(socket2);
            FConnections.Add(conn);
        }

        public void Save(string filename)
        {
            Dependencies.Clear();
            foreach (var node in Nodes)
            {
                Dependencies.Add(node.GetType().Assembly);
                foreach (var socket in node.Inputs)
                {
                    Dependencies.Add(socket.DataType.Assembly);
                }
                foreach (var socket in node.Outputs)
                {
                    Dependencies.Add(socket.DataType.Assembly);
                }
            }
            
            using (var writer = new StreamWriter(filename))
            using (var json = new JsonTextWriter(writer))
            {
                Serializer.Serialize(json, this);
            }
        }
    }
}

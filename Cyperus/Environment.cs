using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IO;
using System.IO;

namespace Cyperus
{
    /// <summary>
    /// Represents an environment for node structures
    /// </summary>
    [Serializable]
    public class Environment
    {
        [NonSerialized]
        static JsonSerializer Serializer = new JsonSerializer();
        
        public List<AbstractNode> Nodes { get; protected set; }
        public ImmutableList<Connection> Connections { get; protected set; }
        public Dictionary<string, Object> Storage { get; protected set; }

        public Environment()
        {
            Nodes = new List<AbstractNode>();
            Connections = ImmutableList.Create<Connection>();
            Storage = new Dictionary<string, Object>();
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
            
            if (Connections.FindIndex(c => ((c.Client == socket1) || (c.Client == socket2)) && ((c.Server == socket1) || (c.Server == socket2))) >= 0)
                return;
            
            var conn = socket1.ConnectTo(socket2);
            Connections = Connections.Add(conn);
        }

        public void Save(string filename)
        {
            using (var writer = new StreamWriter(filename))
            using (var json = new JsonTextWriter(writer))
            {
                Serializer.Serialize(json, this);
            }
        }
    }
}

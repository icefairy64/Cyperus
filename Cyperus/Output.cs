using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Cyperus
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn, IsReference = true)]
    public abstract class Output : AbstractNode
    {
        public Output(string name, Environment env)
            : base(name, env)
        {

        }
    }
}

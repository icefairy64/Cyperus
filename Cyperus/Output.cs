using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyperus
{
    [Serializable]
    public abstract class Output : AbstractNode
    {
        public Output(string name, Environment env)
            : base(name, env)
        {

        }
    }
}

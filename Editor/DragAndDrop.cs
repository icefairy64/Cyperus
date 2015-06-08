using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyperus.Designer
{
    internal struct NodeCreationContainer
    {
        public readonly Type NodeType;

        public NodeCreationContainer(Type nodeType)
        {
            NodeType = nodeType;
        }
    }
}

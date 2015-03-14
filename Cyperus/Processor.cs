using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyperus
{
    [Serializable]
    public abstract class Processor : AbstractNode, ISender
    {
        public Processor(string name, Environment env)
            : base(name, env)
        {
            
        }
        
        public override async Task AcceptData(ISender sender, Object data)
        {
            Object result = await ProcessData(data);
            await DispatchData(sender, result);
        }

        protected abstract Task<Object> ProcessData(Object data);
        protected abstract Task DispatchData(ISender sender, Object data);
    }
}

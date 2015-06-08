using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Cyperus
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn, IsReference = true)]
    public abstract class Processor : AbstractNode, ISender
    {
        public Processor(string name, Environment env)
            : base(name, env)
        {
            
        }
        
        public override async Task AcceptData(ISender sender, Object data)
        {
            if (OnSocketActivity != null)
                OnSocketActivity(this, sender);
            
            Object result = await ProcessData(data);
            await DispatchData(sender, result);
        }

        protected abstract Task<Object> ProcessData(Object data);

        /// <summary>
        /// Dispatches data generated on data received from sender. This method should call OnSocketActivity for any socket that it will send data to.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="data">Generated data</param>
        /// <returns></returns>
        protected abstract Task DispatchData(ISender sender, Object data);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cyperus;

namespace Cyperus.Tests
{
    class TestProcessor : Processor
    {
        Socket<int> Src;
        Socket<int> Dest;

        public TestProcessor(String name, Environment env)
            : base(name, env)
        {
            Src = AddInput<int>("Src");
            Dest = AddOutput<int>("Dest");
        }

        protected override async Task<object> ProcessData(ISender sender, object data)
        {
            Console.WriteLine("Processing: {0} -> {1}", (int)data, 2 * (int)data);
            return 2 * (int)data;
        }

        protected override async Task DispatchData(ISender sender, object data)
        {
            if (sender == Src)
            {
                await Dest.AcceptData(this, data);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cyperus;

namespace Cyperus.Tests
{
    class TestConsumer : Output
    {
        Socket<int> Source;
        
        public int Buffer;

        public TestConsumer(String name, Environment env)
            : base(name, env)
        {
            Source = AddInput<int>("Numbers");
            Console.WriteLine("Created consumer");
        }
        
        public override async Task AcceptData(ISender sender, object data)
        {
            Console.WriteLine(String.Format("In: {0}", data));
            
            if (sender != Source)
            {
                return;
            }

            Buffer += (int)data;
        }
    }
}

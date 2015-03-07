using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyperus
{
    public class TypeMismatchException : Exception
    {
        public TypeMismatchException(string message)
            : base(message)
        {

        }
    }
}

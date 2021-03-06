﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyperus
{
    /// <summary>
    /// Represents an object that can accept data from various sockets
    /// </summary>
    public interface IAcceptor
    {
        Task AcceptData(ISender sender, Object data);
    }

    /// <summary>
    /// Represents an object that can send data to various sockets
    /// </summary>
    public interface ISender
    {
        
    }
}

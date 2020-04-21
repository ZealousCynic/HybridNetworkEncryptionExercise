using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkTCPServer
{
    enum SignalByte
    {
        UNSET,
        MESSAGE,
        IV = 12,
        KEY = 42
    }
}

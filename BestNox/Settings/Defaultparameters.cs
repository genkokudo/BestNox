using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BestNox.Settings
{
    public class Rootobject
    {
        public DefaultParameters DefaultParameter { get; set; }
    }

    public class DefaultParameters
    {
        public object[][] DefaultValues { get; set; }
    }
}

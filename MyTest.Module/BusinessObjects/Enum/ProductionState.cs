﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTest.Module.BusinessObjects.Enum
{
    public enum ProductionState
    {
        workingVersion = 0,
        forProduction = 10,
        waitningForTransport = 20,
        orderSent = 30,

    }
}

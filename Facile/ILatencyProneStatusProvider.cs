﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facile
{
    public interface ILatencyProneStatusProvider
    {
        bool IsLatencyProne { get; }
    }
}
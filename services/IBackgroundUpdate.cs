﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ts.services
{
    public interface IBackgroundUpdate
    {
        Task Update(long userid);
    }
}

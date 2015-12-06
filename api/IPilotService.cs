﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ts.db;

namespace ts.api
{
    public interface IPilotService
    {
        /// <returns>Returns list of PilotInfo, CorporationInfo, CachedUntilUTC, trained skills</returns>
        Task<Tuple<List<Pilot>, List<Corporation>, DateTime>> Get(long userid);
    }
}

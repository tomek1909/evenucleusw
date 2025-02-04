﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ts.domain;

namespace ts.data
{
    public class UserDataDto
    {
        public List<Pilot> Pilots { get; set; }
        public List<Corporation> Corporations { get; set; }
        public List<Job> Jobs { get; set; }
        public DateTime CachedUntilUTC { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ts.dto
{
    public class SkillDto
    {
        public int SkillId { get; set; }
        public long PilotId { get; set; }
        public string SkillName { get; set; }
        public int Level { get; set; }
    }
}

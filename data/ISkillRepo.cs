﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ts.domain;

namespace ts.data
{
    public interface ISkillRepo
    {
        Task Update(long userId, UserDataDto data);
        Task<List<string>> GetForPilot(long eveId);
    }
}

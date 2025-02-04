﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eZet.EveLib.Core.Cache;
using ts.data;
using Serilog;

namespace ring0
{
    public class TestBase
    {
        public AccountContextProvider AccountContextProvider
        {
            get
            {
                var ctxprovider = new AccountContextProvider();

                return ctxprovider;
            }
        }

        public AccountRepo AccountRepo
        {
            get
            {
                var accountrepo = new AccountRepo(AccountContextProvider);

                return accountrepo;
            }
        }

        public static ILogger Logger
        {
            get
            {
                var log = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.Trace().CreateLogger();
                return log;
            }
        }

        public IEveLibCache EveCache => new EveSqlServerCache(Logger, AccountContextProvider);
        public ITypeNameDict TypeNameDict => new TypeNameDict(Logger);
        public ICacheLocalProvider CacheLocalProvider => new CacheLocalProvider(EveCache);
        public IRefTypeDict RefTypeDict => new RefTypeDict(CacheLocalProvider, EveCache);
        public EveApi EveApi => new EveApi(Logger, EveCache);
        public ICharacterNameDict CharacterNameDict => new CharacterNameDict(CacheLocalProvider, EveCache);
    }
}

﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Nancy;
using ts.data;
using ts.domain;
using Session = Nancy.Session.Session;

namespace ts.api
{
    public class BeforeRequest
    {
        public static async Task<Response> BeforeRequestHandler(IAccountRepo accountRepo, NancyContext ctx, CancellationToken token)
        {
            if (!ctx.Request.Url.ToString().Contains("/api/"))
                return null;

            // do not check for session key if login
            if (ctx.Request.Url.ToString().Contains("/account/"))
                return null;

            // check if header contains session key
            var skeys = ctx.Request.Headers["jwt"].ToList();
            if (skeys.Count != 1)
                throw new UserException(strings.InvalidSessionKey);

            // request ctx 
            var userid = await accountRepo.CheckSession(skeys[0]);
            ctx.Request.Session = new Session();
            ctx.Request.Session["UserId"] = userid;
            return null;
        }
    }
}

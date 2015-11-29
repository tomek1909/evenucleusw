﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy.Bootstrapper;
using Nancy;
using Ninject;
using ts.shared;

namespace ts.api
{
    public class Bootstrapper: Nancy.Bootstrappers.Ninject.NinjectNancyBootstrapper
    {
        protected override void ApplicationStartup(IKernel container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);
            StaticConfiguration.EnableRequestTracing = true;
        }

        protected override void ConfigureApplicationContainer(IKernel existingContainer)
        {
            // Perform registation that should have an application lifetime
            existingContainer.Bind<IAccountContextProvider>().To<AccountContextProvider>();
            existingContainer.Bind<IMyConfiguration>().To<MyConfiguration>();
            existingContainer.Bind<IAccountRepo>().To<AccountRepo>();
        }

        protected override void RequestStartup(IKernel container, IPipelines pipelines, NancyContext context)
        {
            pipelines.OnError.AddItemToEndOfPipeline((z, a) => ErrorResponse.FromException(a));

            var accountRepo = container.Get<IAccountRepo>();
            pipelines.BeforeRequest.AddItemToStartOfPipeline((ctx, token) => BeforRequest.BeforRequestHandler(accountRepo, ctx, token));

            base.RequestStartup(container, pipelines, context);
        }
    }
}

using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Auto.Plugins.Base
{
    public class PluginBase
    {
        protected ITracingService TraceService { get; set; }
        protected IPluginExecutionContext PluginContext { get; set; }
        protected IOrganizationService Service { get; set; }
        protected void InitializeComponents(IServiceProvider serviceProvider)
        {
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));

            Service = serviceFactory.CreateOrganizationService(Guid.Empty);
            PluginContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            TraceService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
        }

    }
}

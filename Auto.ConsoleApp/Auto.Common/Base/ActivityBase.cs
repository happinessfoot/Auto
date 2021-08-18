using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;

namespace Auto.Common.Base
{
    public abstract class ActivityBase : CodeActivity
    {
        protected ITracingService TraceService { get; set; }
        protected IOrganizationService Service { get; set; }
        protected IOrganizationServiceFactory ServiceFactory { get; set; }

        protected void InitializeComponent(CodeActivityContext context)
        {
            ServiceFactory = context.GetExtension<IOrganizationServiceFactory>();
            TraceService = context.GetExtension<ITracingService>();
            Service = ServiceFactory.CreateOrganizationService(null);
        }

    }
}

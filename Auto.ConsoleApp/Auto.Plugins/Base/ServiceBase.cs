using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace Auto.Plugins.Base
{
    public abstract class ServiceBase
    {
        protected readonly IOrganizationService service;
        protected readonly ITracingService traceService;
        public ServiceBase(IOrganizationService service, ITracingService traceService)
        {
            this.service = service;
            this.traceService = traceService;
        }
    }
}

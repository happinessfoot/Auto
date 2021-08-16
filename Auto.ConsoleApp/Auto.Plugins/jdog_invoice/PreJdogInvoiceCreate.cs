using Auto.Plugins.Base;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auto.Plugins.jdog_invoice.Handlers;
using Auto.Common.Entities;

namespace Auto.Plugins.jdog_invoice
{
    public sealed class PreJdogInvoiceCreate : PluginBase,IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            InitializeComponents(serviceProvider);

            //ITracingService traceService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            //traceService.Trace("Additinal data");

            //IPluginExecutionContext pluginContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            //Entity targetAccount = (Entity)pluginContext.InputParameters["Target"];

            //string name = targetAccount.GetAttributeValue<string>("name");
            //traceService.Trace("Имя организации:" + name);

            //IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            //IOrganizationService service = serviceFactory.CreateOrganizationService(Guid.Empty);
            Entity targetInvoice = (Entity)PluginContext.InputParameters["Target"];
            try
            {
                JdogInvoiceService invoiceService = new JdogInvoiceService(Service, TraceService);
                invoiceService.SetInvoiceType(targetInvoice);
            }
            catch(Exception ex)
            {
                throw new InvalidPluginExecutionException(ex.Message);
            }



            //throw new InvalidPluginExecutionException(name+"My exception 0-0");

        }
    }
}

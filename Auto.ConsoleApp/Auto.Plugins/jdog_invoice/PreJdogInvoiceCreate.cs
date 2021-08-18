using Auto.Common.Base;
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

            Entity targetInvoice = (Entity)PluginContext.InputParameters["Target"];
            try
            {
                JdogInvoiceService invoiceService = new JdogInvoiceService(Service, TraceService);
                invoiceService.SetInvoiceType(targetInvoice);
                invoiceService.AddPaidValue(targetInvoice);
            }
            catch(Exception ex)
            {
                throw new InvalidPluginExecutionException(ex.Message);
            }

        }
    }
}

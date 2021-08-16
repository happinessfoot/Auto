using Auto.Plugins.Base;
using Auto.Plugins.jdog_invoice.Handlers;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auto.Plugins.jdog_invoice
{
    public sealed class PreJdogInvoiceDelete : PluginBase, IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            InitializeComponents(serviceProvider);

            EntityReference targetInvoice = (EntityReference)PluginContext.InputParameters["Target"];
            try
            {
                JdogInvoiceService invoiceService = new JdogInvoiceService(Service, TraceService);
                invoiceService.DeleteInvoice(targetInvoice);
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException(ex.Message);
            }

        }
    }
}

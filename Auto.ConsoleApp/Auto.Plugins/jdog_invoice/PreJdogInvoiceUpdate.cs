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
    public sealed class PreJdogInvoiceUpdate : PluginBase, IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            InitializeComponents(serviceProvider);
            Entity targetInvoicement = (Entity)PluginContext.InputParameters["Target"];
            try
            {
                JdogInvoiceService jdogInvoiceService = new JdogInvoiceService(Service, TraceService);
                jdogInvoiceService.CalculateAmounCredit(targetInvoicement);
                //jdogAgreementService.SetFirstDateAgreement(targetAgreement);

            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException(ex.Message);
            }
        }
    }
}

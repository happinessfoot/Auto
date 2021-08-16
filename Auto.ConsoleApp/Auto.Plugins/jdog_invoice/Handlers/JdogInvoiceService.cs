using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auto.Common.Commands;
using System.Windows.Input;
using Auto.Common.Entities;
using Auto.Plugins.Base;

namespace Auto.Plugins.jdog_invoice.Handlers
{
    public class JdogInvoiceService : ServiceBase
    {
        public JdogInvoiceService(IOrganizationService service,ITracingService traceService) : base(service,traceService)
        {

        }
        public void SetInvoiceType(Entity targetInvoice)
        {
            try
            {
                SetInvoiceTypeCommand setInvoiceTypeCommand = new SetInvoiceTypeCommand();
                setInvoiceTypeCommand.Execute(targetInvoice);
            }
            catch(Exception ex)
            {
                traceService.Trace(ex.ToString());
                throw ex;
            }
        }
    }

}

using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auto.Common.Entities;
using System.Windows.Input;

namespace Auto.Common.Commands
{
    public class SetInvoiceTypeCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object targetInvoice)
        {
            return targetInvoice == null || !(targetInvoice is Entity);
        }


        public void Execute(object targetInvoice)
        {
            Entity invoice = (Entity)targetInvoice;
            if (!invoice.Attributes.ContainsKey("jdog_type"))
            {
                invoice["jdog_type"] =  new OptionSetValue((int)jdog_invoice_jdog_type.__378860000);
            }
        }
    }
}

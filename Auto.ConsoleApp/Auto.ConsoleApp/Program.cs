using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Auto.Common.Commands;
using System.Configuration;
using Auto.Plugins.jdog_agreement.Handlers;
using Auto.Plugins.jdog_invoice.Handlers;

namespace Auto.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            string connectionString = ConfigurationManager.ConnectionStrings["connectionCRM"].ConnectionString;

            CrmServiceClient client = new CrmServiceClient(connectionString);
            if (client.LastCrmException != null)
            {
                System.Console.WriteLine(client.LastCrmError);
                System.Console.WriteLine(client.LastCrmException);
            }
            IOrganizationService service = (IOrganizationService)client;
            Entity invoice = service.Retrieve("jdog_invoice", Guid.Parse("A06AEBAC-94FE-EB11-94EF-000D3A206C55"), new ColumnSet("jdog_fact"));
            invoice["jdog_fact"] = true;
            JdogInvoiceService jdogInvoiceService = new JdogInvoiceService(service, new TraceServiceConsole());
            jdogInvoiceService.CalculateAmounCredit(invoice);
            //SetInvoiceTypeCommand setInvoiceTypeCommand = new SetInvoiceTypeCommand();
            //setInvoiceTypeCommand.Execute(invoice);
            //Entity entity = service.Retrieve("contact", Guid.Parse("F8428F3E-E5F6-EB11-94EF-00224881A0DB"), new ColumnSet("contactid"));
            //JdogAgreementService contactService = new JdogAgreementService(service, new TraceServiceConsole());
            //contactService.SetFirstDateAgreement(entity);

            //service.Update(invoiceUpdate);

            Console.Read();
        }
    }
}

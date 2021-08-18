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
using Auto.Common.Entities;
using Auto.Plugins.jdog_communication.Handlers;

namespace Auto.Common
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
            
            //Entity invoice = service.Retrieve("jdog_communication", Guid.Parse("FCC7F26D-AEFE-EB11-94EF-000D3A206C55"), new ColumnSet(true));
            //invoice["jdog_fact"] = true;
            jdog_communication communication = new jdog_communication();
            communication.Id = Guid.Parse("6439A6F7-B2FE-EB11-94EF-000D3A206C55");
            communication.jdog_contactid = service.Retrieve("contact",Guid.Parse("1B563A9F-B1F6-EB11-94EF-00224881A0DB"),new ColumnSet(true)).ToEntityReference();
            communication.jdog_type = jdog_communication_jdog_type.Email;
            communication.jdog_main = true;
            JdogCommunicationService communicationService = new JdogCommunicationService(service, new TraceServiceConsole());
            communicationService.ChangeToMain(communication);
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

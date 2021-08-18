using Auto.Common.Base;
using Auto.Common.Entities;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk.Query;

namespace Auto.Workflows.JdogInvoiceActivities.Handlers
{
    public class JdogInvoiceActivityService : ServiceBase
    {
        private void CreateCustomInvoice(EntityReference agreementRef, string invoiceName, Money invoiceAmount,DateTime invoiceDate)
        {
            jdog_invoice invoiceNew = new jdog_invoice();
            invoiceNew.jdog_dogovorid = agreementRef;
            invoiceNew.jdog_name = invoiceName; 
            invoiceNew.jdog_type = jdog_invoice_jdog_type.__378860001;
            invoiceNew.jdog_amount = invoiceAmount;
            invoiceNew.jdog_date = invoiceDate;
            invoiceNew.jdog_fact = false;
            service.Create(invoiceNew);
        }
        public JdogInvoiceActivityService(IOrganizationService service,ITracingService tracingService):base(service,tracingService)
        {

        }
        public void CreateInvoice(EntityReference agreementRef)
        {
            try
            {
                jdog_agreement agreement = service.Retrieve(agreementRef.LogicalName, agreementRef.Id, new ColumnSet("jdog_fullcreditamount")).ToEntity<jdog_agreement>();
                CreateCustomInvoice(agreementRef, $"СЧЕТ №{Environment.TickCount.ToString().Replace('.', '_')}", agreement.jdog_fullcreditamount,DateTime.Today);
            }
            catch(Exception ex)
            {
                traceService.Trace(ex.ToString());
                throw ex;
            }
        }
        public void DeleteAllInvoice(EntityReference agreementRef)
        {
            try
            {
                QueryExpression query = new QueryExpression("jdog_invoice");
                query.Criteria.AddCondition("jdog_dogovorid", ConditionOperator.Equal, agreementRef.Id);
                query.Criteria.AddCondition("jdog_type", ConditionOperator.Equal, (int)jdog_invoice_jdog_type.__378860001);
                query.ColumnSet = new ColumnSet("jdog_invoiceid");
                EntityCollection result = service.RetrieveMultiple(query);
                foreach(jdog_invoice invoice in result.Entities.Select(e=>e.ToEntity<jdog_invoice>()))
                {
                    service.Delete(invoice.LogicalName, invoice.Id);
                }

            }
            catch (Exception ex)
            {
                traceService.Trace(ex.ToString());
                throw ex;
            }
        }
        public void CreatePaymentPeriod(EntityReference agreementRef)
        {
            try
            {
                jdog_agreement agreement = service.Retrieve(agreementRef.LogicalName, agreementRef.Id, new ColumnSet("jdog_fullcreditamount", "jdog_creditperiod","jdog_date")).ToEntity<jdog_agreement>();
                int creditPeriodMonth = (int)(agreement.jdog_creditperiod * 12);
                decimal invoiceAmount = agreement.jdog_fullcreditamount.Value/creditPeriodMonth;
                DateTime payDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month,1);
                for (int i = 0; i<creditPeriodMonth;i++)
                {
                    payDate=payDate.AddMonths(i + 1);
                    CreateCustomInvoice(agreementRef, $"СЧЕТ №{Environment.TickCount.ToString().Replace('.', '_')}_{(i + 1)}", new Money(invoiceAmount), payDate);
                }
            }
            catch (Exception ex)
            {
                traceService.Trace(ex.ToString());
                throw ex;
            }
        }
    }
}

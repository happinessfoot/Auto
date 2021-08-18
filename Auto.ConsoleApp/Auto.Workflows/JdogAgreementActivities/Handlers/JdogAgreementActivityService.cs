using Auto.Common.Base;
using Auto.Common.Entities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auto.Workflows.JdogAgreementActivities.Handlers
{
    public class JdogAgreementActivityService : ServiceBase
    {
        public JdogAgreementActivityService(IOrganizationService service,ITracingService tracingService) : base(service,tracingService)
        {

        }
        public bool CheckInvoiceInAgreement(EntityReference agreementRef)
        {
            try
            {
                QueryExpression query = new QueryExpression("jdog_invoice");
                query.ColumnSet = new ColumnSet("jdog_invoiceid");
                query.Criteria.AddCondition("jdog_dogovorid", ConditionOperator.Equal, agreementRef.Id);
                query.TopCount = 1;
                EntityCollection result = service.RetrieveMultiple(query);
                return result.Entities.Count == 1;
            }
            catch (Exception ex)
            {
                traceService.Trace(ex.ToString());
                throw ex;
            }
        }
        public bool CheckPaidInvoiceInAgreement(EntityReference agreementRef)
        {
            try
            {
                QueryExpression query = new QueryExpression("jdog_invoice");
                query.ColumnSet = new ColumnSet("jdog_invoiceid");
                query.Criteria.AddCondition("jdog_dogovorid", ConditionOperator.Equal, agreementRef.Id);
                query.Criteria.AddCondition("jdog_fact", ConditionOperator.Equal, true);
                query.TopCount = 1;
                EntityCollection result = service.RetrieveMultiple(query);
                return result.Entities.Count == 1;
            }
            catch (Exception ex)
            {
                traceService.Trace(ex.ToString());
                throw ex;
            }
        }
        public bool CheckManualInvoiceInAgreement(EntityReference agreementRef)
        {
            try
            {
                QueryExpression query = new QueryExpression("jdog_invoice");
                query.ColumnSet = new ColumnSet("jdog_invoiceid");
                query.Criteria.AddCondition("jdog_dogovorid", ConditionOperator.Equal, agreementRef.Id);
                query.Criteria.AddCondition("jdog_type", ConditionOperator.Equal, (int)jdog_invoice_jdog_type.__378860000);
                query.TopCount = 1;
                EntityCollection result = service.RetrieveMultiple(query);
                return result.Entities.Count == 1;
            }
            catch (Exception ex)
            {
                traceService.Trace(ex.ToString());
                throw ex;
            }
        }
        public void SetDatePeriod(EntityReference agreementRef)
        {
            try
            {
                jdog_agreement agreementUpdate = new jdog_agreement();
                agreementUpdate.Id = agreementRef.Id;
                agreementUpdate.jdog_paymentplandate = DateTime.Now.AddDays(1);
                service.Update(agreementUpdate);
            }
            catch (Exception ex)
            {
                traceService.Trace(ex.ToString());
                throw ex;
            }
        }
    }
}

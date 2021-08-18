using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auto.Common.Base;
using Auto.Common.Entities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

using ContactEntity = Auto.Common.Entities.Contact;

namespace Auto.Plugins.jdog_agreement.Handlers
{
    public class JdogAgreementService : ServiceBase
    {
        public JdogAgreementService(IOrganizationService service, ITracingService traceService) : base(service, traceService)
        {

        }
        public void SetFirstDateAgreement(Entity targetAgreement)
        {
            try
            {
                Common.Entities.jdog_agreement agreement = targetAgreement.ToEntity<Common.Entities.jdog_agreement>();
                QueryExpression query = new QueryExpression("jdog_agreement");
                query.ColumnSet = new ColumnSet("jdog_date");
                query.NoLock = true;
                query.TopCount = 1;
                query.Criteria.AddCondition(new ConditionExpression("jdog_contact", ConditionOperator.Equal, agreement.jdog_contact.Id));
                EntityCollection result = service.RetrieveMultiple(query);
                if (result.Entities.Count == 0)
                {
                    ContactEntity contact = new ContactEntity();
                    contact.Id = agreement.jdog_contact.Id;
                    contact.jdog_date = agreement.jdog_date;
                    service.Update(contact);
                }
            }
            catch(Exception ex)
            {
                traceService.Trace(ex.ToString());
                throw ex;
            }
        }
    }
}

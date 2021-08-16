using Auto.Plugins.Base;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auto.Common.Entities;

using JdogCommunication = Auto.Common.Entities.jdog_communication;

namespace Auto.Plugins.jdog_communication.Handlers
{
    public class JdogCommunicationService : ServiceBase
    {
        private void CheckingMainType(Entity targetCommunication, EntityReference contact,OptionSetValue communicationType)
        {
            QueryExpression query = new QueryExpression("jdog_communication");
            query.ColumnSet = new ColumnSet("jdog_main", "jdog_type");
            LinkEntity linkEntity = query.AddLink("contact", "jdog_contactid", "contactid");
            linkEntity.LinkCriteria.AddCondition("contactid", ConditionOperator.Equal, contact.Id);
            query.Criteria.AddCondition("jdog_main", ConditionOperator.Equal, true);
            query.Criteria.AddCondition("jdog_type", ConditionOperator.NotNull);
            query.Distinct = true;
            EntityCollection result = service.RetrieveMultiple(query);
            if (result.Entities.Count > 1)
            {
                throw new InvalidPluginExecutionException($"У контакта {contact.Name} уже имеются основные телефон и email");
            }
            foreach (JdogCommunication communication in result.Entities.Select(e => e.ToEntity<JdogCommunication>()))
            {
                if ((int?)communication.jdog_type.Value == communicationType.Value)
                {
                    throw new InvalidPluginExecutionException($"У контакта {contact.Name} уже имеется такой основной тип связи");
                }
            }
        }

        public JdogCommunicationService(IOrganizationService service, ITracingService traceService) : base(service, traceService)
        {

        }
        public void CreateCommunication(Entity targetCommunication)
        {
            try
            {
                if (((bool?)targetCommunication["jdog_main"]) == true)
                {
                    CheckingMainType(targetCommunication, (EntityReference)targetCommunication["jdog_contactid"],(OptionSetValue)targetCommunication["jdog_type"]);
                }
                
            }
            catch (Exception ex)
            {
                traceService.Trace(ex.ToString());
                throw ex;
            }
        }
        public void ChangeToMain(Entity targetCommunication)
        {
            try
            {
                if (((bool?)targetCommunication["jdog_main"]) == true)
                {
                    OptionSetValue communicationType = null;

                    JdogCommunication jdogCommunicationOld = service.Retrieve("jdog_communication", (Guid)targetCommunication["jdog_communicationid"], new ColumnSet("jdog_contactid","jdog_type")).ToEntity<JdogCommunication>();
                    if (targetCommunication.Attributes.ContainsKey("jdog_type"))
                    {
                        communicationType = (OptionSetValue)targetCommunication.Attributes["jdog_type"];
                    }
                    else
                    {
                        if(jdogCommunicationOld.jdog_type == null)
                        {
                            throw new InvalidPluginExecutionException("Не выбран тип связи");
                        }
                        communicationType = new OptionSetValue(((int)jdogCommunicationOld.jdog_type.Value));
                    }
                    CheckingMainType(targetCommunication, jdogCommunicationOld.jdog_contactid, communicationType);
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

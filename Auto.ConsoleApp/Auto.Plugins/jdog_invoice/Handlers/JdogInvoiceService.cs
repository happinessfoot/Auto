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
using Microsoft.Xrm.Sdk.Query;

using JdogAgreement = Auto.Common.Entities.jdog_agreement;
using JdogInvoice = Auto.Common.Entities.jdog_invoice;

namespace Auto.Plugins.jdog_invoice.Handlers
{
    public class JdogInvoiceService : ServiceBase
    {
        private void Setjdog_factsumma(JdogAgreement agreementUpdate,Money fullCreditAmount, Money newFactSumma)
        {
            if (newFactSumma.Value > fullCreditAmount.Value)
            {
                throw new InvalidPluginExecutionException($"Сумма всех оплаченных счетов превысила полную стоимость кредита.");
            }
            else
            {

                if (fullCreditAmount.Value == newFactSumma.Value)
                {
                    agreementUpdate.jdog_fact = true;
                }
                agreementUpdate.jdog_factsumma = newFactSumma;
            }
        }
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
        public void AddPaidValue(Entity targetInvoice)
        {
            try
            {
                if (((bool?)targetInvoice["jdog_fact"])==true)
                {
                    if (targetInvoice.Attributes.ContainsKey("jdog_amount"))
                    {
                        JdogAgreement agreement = service.Retrieve(JdogAgreement.EntityLogicalName, ((EntityReference)targetInvoice["jdog_dogovorid"]).Id, new ColumnSet(nameof(agreement.jdog_factsumma), nameof(agreement.jdog_fullcreditamount))).ToEntity<JdogAgreement>();
                        Money factSumma = agreement.jdog_factsumma != null ? agreement.jdog_factsumma : new Money(0);
                        Money fullCreditAmount = agreement.jdog_fullcreditamount!=null ? agreement.jdog_fullcreditamount : new Money(0);
                        Money invoiceAmount = (Money)targetInvoice["jdog_amount"];
                        Money newAmount = new Money(invoiceAmount.Value + factSumma.Value);
                        JdogAgreement agreementUpdate = new JdogAgreement { Id = agreement.Id };
                        try
                        {
                            Setjdog_factsumma(agreementUpdate, fullCreditAmount, newAmount);
                            targetInvoice["jdog_paydate"] = DateTime.Today;
                        }
                        catch(Exception ex)
                        {
                            throw new InvalidPluginExecutionException(ex.Message + $"\nПолная стоимость кредита:{fullCreditAmount.Value}\nСумма всех счетов (включая текущий):{newAmount.Value}\nСчет:{invoiceAmount.Value}");
                        }
                        service.Update(agreementUpdate);
                    }
                    else
                    {
                        throw new InvalidPluginExecutionException($"Отсутствует сумма!");
                    }
                }
            }
            catch(Exception ex)
            {
                traceService.Trace(ex.ToString());
                throw ex;
            }
        }
        public void CalculateAmounCredit(Entity targetInvoice)
        {
            try
            {

                JdogInvoice jdogInvoiceOld = service.Retrieve(JdogInvoice.EntityLogicalName, targetInvoice.Id, new ColumnSet("jdog_dogovorid", "jdog_amount")).ToEntity<JdogInvoice>();
                if (((bool?)targetInvoice["jdog_fact"]) == true)
                {
                    JdogAgreement agreement = service.Retrieve(JdogAgreement.EntityLogicalName, jdogInvoiceOld.jdog_dogovorid.Id, new ColumnSet(nameof(agreement.jdog_factsumma), nameof(agreement.jdog_fullcreditamount))).ToEntity<JdogAgreement>();
                    Money fullCreditAmount = agreement.jdog_factsumma != null ? agreement.jdog_fullcreditamount : new Money(0);
                    decimal newAmount = 0;
                    JdogAgreement agreementUpdate = new JdogAgreement { Id = agreement.Id };
                    QueryExpression query = new QueryExpression("jdog_invoice");
                    query.ColumnSet = new ColumnSet("jdog_amount");
                    LinkEntity linkEntity = query.AddLink(JdogAgreement.EntityLogicalName, "jdog_dogovorid", "jdog_agreementid");
                    linkEntity.EntityAlias = "ia";
                    linkEntity.LinkCriteria.AddCondition("jdog_agreementid", ConditionOperator.Equal, agreement.Id);
                    query.Criteria.AddCondition("jdog_fact", ConditionOperator.Equal, true);
                    query.Criteria.AddCondition("jdog_amount", ConditionOperator.NotNull);
                    query.Criteria.AddCondition("jdog_invoiceid",ConditionOperator.NotEqual, targetInvoice.Id);

                    EntityCollection result = service.RetrieveMultiple(query);
                    foreach (JdogInvoice invoice in result.Entities.Select(e => e.ToEntity<JdogInvoice>()))
                    {
                        newAmount += invoice.jdog_amount.Value;
                    }
                    try
                    {
                        if(targetInvoice.Attributes.ContainsKey("jdog_amount"))
                        {
                            newAmount += ((Money)targetInvoice["jdog_amount"]).Value;
                        }
                        else
                        {
                            newAmount += jdogInvoiceOld.jdog_amount.Value;
                        }
                        Setjdog_factsumma(agreementUpdate, fullCreditAmount, new Money(newAmount));
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidPluginExecutionException(ex.Message + $"\nПолная стоимость кредита:{fullCreditAmount.Value}\nСумма всех счетов (включая текущий):{newAmount}");
                    }
                    service.Update(agreementUpdate);
                }
                
            }
            catch(Exception ex)
            {
                traceService.Trace(ex.ToString());
                throw ex;
            }
        }
        public void DeleteInvoice(EntityReference targetInvoice)
        {
            try
            {
                JdogInvoice jdogInvoiceOld = service.Retrieve(JdogInvoice.EntityLogicalName, targetInvoice.Id, new ColumnSet("jdog_dogovorid","jdog_amount")).ToEntity<JdogInvoice>();
                JdogAgreement agreement = service.Retrieve(JdogAgreement.EntityLogicalName, jdogInvoiceOld.jdog_dogovorid.Id, new ColumnSet(nameof(agreement.jdog_factsumma), nameof(agreement.jdog_fullcreditamount))).ToEntity<JdogAgreement>();
                JdogAgreement agreementUpdate = new JdogAgreement { Id = agreement.Id };
                agreementUpdate.jdog_factsumma = new Money(agreement.jdog_factsumma != null ? agreement.jdog_factsumma.Value - jdogInvoiceOld.jdog_amount.Value : 0);
                service.Update(agreementUpdate);
            }
            catch(Exception ex)
            {
                traceService.Trace(ex.ToString());
                throw ex;
            }
        }
    }

}

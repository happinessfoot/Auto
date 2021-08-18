using Auto.Common.Base;
using Auto.Workflows.JdogAgreementActivities.Handlers;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auto.Workflows.JdogAgreementActivities
{
    public class AgreementSetDatePeriodActivity : ActivityBase
    {
        [Input("Договор")]
        [RequiredArgument]
        [ReferenceTarget("jdog_agreement")]
        public InArgument<EntityReference> JdogAgreementReference { get; set; }
        protected override void Execute(CodeActivityContext context)
        {
            InitializeComponent(context);
            try
            {
                JdogAgreementActivityService agreementService = new JdogAgreementActivityService(Service, TraceService);
                agreementService.SetDatePeriod(JdogAgreementReference.Get(context));
            }
            catch (Exception ex)
            {
                throw new InvalidWorkflowException(ex.Message);
            }
        }
    }
}

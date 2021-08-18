using System.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using Auto.Workflows.JdogInvoiceActivities.Handlers;
using Auto.Common.Base;
using Auto.Workflows.JdogAgreementActivities.Handlers;

namespace Auto.Workflows.JdogAgreementActivities
{
    public sealed class AgreementHaveInvoiceActivity : ActivityBase
    {
        [Input("Договор")]
        [RequiredArgument]
        [ReferenceTarget("jdog_agreement")]
        public InArgument<EntityReference> JdogAgreementReference { get; set; }
        [Output("Есть счет")]
        public OutArgument<bool> HasInvoice { get; set; }
        protected override void Execute(CodeActivityContext context)
        {
            InitializeComponent(context);
            try
            {
                JdogAgreementActivityService agreementService = new JdogAgreementActivityService(Service, TraceService);
                HasInvoice.Set(context,agreementService.CheckInvoiceInAgreement(JdogAgreementReference.Get(context)));
            }
            catch(Exception ex)
            {
                throw new InvalidWorkflowException(ex.Message);
            }
        }
    }
}

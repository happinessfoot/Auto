﻿using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auto.Common.Entities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using Auto.Workflows.JdogInvoiceActivities.Handlers;
using Auto.Common.Base;

namespace Auto.Workflows.JdogInvoiceActivities
{
    public sealed class InvoiceCreatePeriodActivity : ActivityBase
    {
        [Input("Agreement")]
        [RequiredArgument]
        [ReferenceTarget("jdog_agreement")]
        public InArgument<EntityReference> JdogAgreementReference { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            InitializeComponent(context);
            //IWorkflowContext wfContext = context.GetExtension<IWorkflowContext>();
            //IOrganizationServiceFactory serviceFactory = context.GetExtension<IOrganizationServiceFactory>();
            //ITracingService tracingService = context.GetExtension<ITracingService>();
            //IOrganizationService service = serviceFactory.CreateOrganizationService(null);
            try
            {
                JdogInvoiceActivityService invoiceService = new JdogInvoiceActivityService(Service, TraceService);
                invoiceService.CreatePaymentPeriod(JdogAgreementReference.Get(context));
            }
            catch (Exception ex)
            {
                throw new InvalidWorkflowException(ex.Message);
            }

        }
    }
}

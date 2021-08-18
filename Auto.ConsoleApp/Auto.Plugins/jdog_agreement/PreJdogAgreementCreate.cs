using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auto.Common;
using Auto.Common.Base;
using Auto.Plugins.jdog_agreement.Handlers;

namespace Auto.Plugins.jdog_agreement
{
    public sealed class PreJdogAgreementCreate : PluginBase, IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            InitializeComponents(serviceProvider);
            Entity targetAgreement = (Entity)PluginContext.InputParameters["Target"];
            try
            {
                JdogAgreementService jdogAgreementService = new JdogAgreementService(Service, TraceService);
                jdogAgreementService.SetFirstDateAgreement(targetAgreement);
               
            }
            catch(Exception ex)
            {
                throw new InvalidPluginExecutionException(ex.Message);
            }
        }
    }
}

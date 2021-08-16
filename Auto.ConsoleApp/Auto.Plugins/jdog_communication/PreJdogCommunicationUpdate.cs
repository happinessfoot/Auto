using Auto.Plugins.Base;
using Auto.Plugins.jdog_communication.Handlers;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auto.Plugins.jdog_communication
{
    public sealed class PreJdogCommunicationUpdate : PluginBase, IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            InitializeComponents(serviceProvider);

            Entity targetCommunication = (Entity)PluginContext.InputParameters["Target"];
            try
            {
                JdogCommunicationService communicationService = new JdogCommunicationService(Service, TraceService);
                communicationService.ChangeToMain(targetCommunication);
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException(ex.Message);
            }

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderClient.ClientDataAccessWCFService;
using OrderClient.Abstract;
using System.ServiceModel;

namespace OrderClient.Service_Communication
{
    class ClientDataAccess : ClientDataAccessWCFServiceClient, IClientDataAccess
    {
        public ClientDataAccess(IOrderNotyficator orderNotyficator) :
            base(new InstanceContext(orderNotyficator)) {}

        private ClientDataAccess(System.ServiceModel.InstanceContext callbackInstance) : 
            base(callbackInstance) { }

        private ClientDataAccess(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName) : 
            base(callbackInstance, endpointConfigurationName) { }

        private ClientDataAccess(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) :
            base(callbackInstance, endpointConfigurationName, remoteAddress) { }

        private ClientDataAccess(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) :
            base(callbackInstance, endpointConfigurationName, remoteAddress) { }

        private ClientDataAccess(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
            base(callbackInstance, binding, remoteAddress) { }
    }
}

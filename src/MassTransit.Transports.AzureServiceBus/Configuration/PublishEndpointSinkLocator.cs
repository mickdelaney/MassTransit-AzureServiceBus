// Copyright 2012 Henrik Feldt
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace MassTransit.Transports.AzureServiceBus.Configuration
{
    using System;
    using Pipeline.Inspectors;
    using Pipeline.Sinks;
    using Util;


    /// <summary>
    /// Finds publish endpoints that consume the message type specified in the c'tor of this
    /// class.
    /// </summary>
    public class PublishEndpointSinkLocator :
        PipelineInspectorBase<PublishEndpointSinkLocator>
    {
        readonly IAzureServiceBusEndpointAddress _endpointAddress;
        readonly Type _messageType;

        /// <summary>
        /// c'tor
        /// </summary>
        public PublishEndpointSinkLocator([NotNull] Type messageType,
            [NotNull] IAzureServiceBusEndpointAddress endpointAddress)
        {
            if (messageType == null)
                throw new ArgumentNullException("messageType");
            if (endpointAddress == null)
                throw new ArgumentNullException("endpointAddress");
            _endpointAddress = endpointAddress;
            _messageType = messageType;
        }

        /// <summary>
        /// Gets whether the publish endpoint sink locator found a message sink
        /// that matched the type passed in the c'tor.
        /// </summary>
        public bool Found { get; private set; }

        /// <summary>
        /// Called by the reflective visitor base.
        /// </summary>
        public bool Inspect<TMessage>(EndpointMessageSink<TMessage> sink)
            where TMessage : class
        {
            if (typeof(TMessage) == _messageType && _endpointAddress.Uri == sink.Endpoint.Address.Uri)
            {
                Found = true;

                return false;
            }

            return true;
        }
    }
}
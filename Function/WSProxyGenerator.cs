
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ServiceModel;
using Microsoft.Azure.Functions.Extensions.Workflows;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using ServiceReference1;
namespace MyNamespace
{
   

    public class AddAsyncLogicAppProxy
    {
        private readonly ILogger<AddAsyncLogicAppProxy> logger;
        public AddAsyncLogicAppProxy(ILoggerFactory loggerFactory)
            {
                logger = loggerFactory.CreateLogger<AddAsyncLogicAppProxy>();
            }
        [FunctionName("AddAsyncLogicAppProxy")]
        public Task<Int32> Run([WorkflowActionTrigger] string endpointAddress, Int32 a, Int32 b)
            {
                var _binding = new BasicHttpsBinding();
                var _address = new EndpointAddress(endpointAddress);
                using (var proxy = new CalculatorWebServiceSoapClient(_binding, _address))
                  {
                      var result =  proxy.AddAsync(a, b);
                      return result;
                  }
            }
    }
   

    public class SubtractAsyncLogicAppProxy
    {
        private readonly ILogger<SubtractAsyncLogicAppProxy> logger;
        public SubtractAsyncLogicAppProxy(ILoggerFactory loggerFactory)
            {
                logger = loggerFactory.CreateLogger<SubtractAsyncLogicAppProxy>();
            }
        [FunctionName("SubtractAsyncLogicAppProxy")]
        public Task<Int32> Run([WorkflowActionTrigger] string endpointAddress, Int32 a, Int32 b)
            {
                var _binding = new BasicHttpsBinding();
                var _address = new EndpointAddress(endpointAddress);
                using (var proxy = new CalculatorWebServiceSoapClient(_binding, _address))
                  {
                      var result =  proxy.SubtractAsync(a, b);
                      return result;
                  }
            }
    }
  
}
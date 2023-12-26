This article shows how to automatically create local function app classes that can be called by logic app .

We'll also talk about the use of T4 for code generation and provide a practical example with a sample web service.

Understanding WorkflowActionTrigger
-----------------------------------

WorkflowActionTrigger is a type of function app triggers that can be invoked by logic app which called **Local Function App**

Image

For more information, visit [.NET Framework Custom Code for Azure Logic Apps (Standard) Reaches General Availability (microsoft.com)](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/net-framework-custom-code-for-azure-logic-apps-standard-reaches/ba-p/3954619).

Integrating Web Services into .NET
----------------------------------

Integrating web services into a .NET project can be done using tools like ServiceUtil and wsdl.exe. 

In my POC I will talk only about ServiceUtil which it will create **reference.cs** class and it is available in **visual studio 2022**

For detailed information on wsdl.exe, visit [WSDL and Service Contracts](https://learn.microsoft.com/en-us/windows/win32/wsw/wsdl-support).

For detailed information on ServiceUtil (Svcutil.exe), visit [ServiceModel Metadata Utility Tool (Svcutil.exe)](https://learn.microsoft.com/en-us/dotnet/framework/wcf/servicemodel-metadata-utility-tool-svcutil-exe).

[https://learn.microsoft.com/en-us/dotnet/core/additional-tools/wcf-web-service-reference-guide](https://learn.microsoft.com/en-us/dotnet/core/additional-tools/wcf-web-service-reference-guide)

Generating C# Code with T4 Templates
------------------------------------

T4 (Text Template Transformation Toolkit) is a powerful tool for generating C# code. It can be run using TextTransform.exe. Here's how to do it:

The template that I have created will open the function app DLL using reflection then scan it for all interfaces attribute named _ServiceContractAttribute_

Then build a new class that going to be consumed by logic app.

The class will look like the below:

```
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
```

Running the Sample
------------------

Since the template expect the Dll to be exist then fist you need to build your Dll first 

You need to locate the file TextTransform.exe which exists

C:\\Program Files\\Microsoft Visual Studio\\2022\\Enterprise\\Common7\\IDE\\TextTransform.exe

Modify the T4 file and change  Dll path inside the template then save it.

string dllPath = "......fa.dll";

Then run the below command to generate the c# file that contains all classes

TextTransform.exe" "path\_to\\WSProxyGenerator.tt"

All steps :
```
#first Build to get the Dll
dotnet build FunctionApp.csproj 
#generate the proxy class CS file 
TextTransform.exe" "path\_to\\WSProxyGenerator.tt" 
#Build to get the Dll again to get the proxy generated 
dotnet build FunctionApp.csproj
```
Make sure that you have the **function.json** in the lib folder in logic app. 

To demonstrate the concepts, we will use a sample web service. The logic app will call the following web service: [Calc.asmx](http://ecs.syr.edu/faculty/fawcett/Handouts/cse775/code/calcWebService/Calc.asmx).

from [http://ecs.syr.edu/faculty/fawcett/Handouts/cse775/code/calcWebService/Calc.asmx](http://ecs.syr.edu/faculty/fawcett/Handouts/cse775/code/calcWebService/Calc.asmx)

To access the complete repository with all the necessary files and detailed instructions, visit the GitHub repository:

 [ logic-app-workspace-with-web-service-client-generator](https://github.com/mbarqawi/logic-app-workspace-with-web-service-client-generator)

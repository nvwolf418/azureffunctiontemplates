using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text;
using Microsoft.Azure.Storage.Blob;

namespace bloboutputbind
{
    /*
     * You need to install the extension binding for v2 and higher->
     * https://www.nuget.org/packages/Microsoft.Azure.WebJobs.Extensions.Storage
     * 
     * This is noted here->
     * https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-storage-blob#add-to-your-functions-app
     */


    /*Please note the second argument below->
       * [Blob("test", System.IO.FileAccess.Write, Connection ="your app setting here")],  
       * you need 3 things:
      * 1) first argument being the path to the container you want to write to
      * 2) second argument being access permissions System.IO.FileAccess.WHATEVER PERMISSION YOU WANT(options being read, write, readwrite)
      * 3) third argument being connection string , an app setting is preferable or a key vault reference
      * 
      * For the variable name, there are only a limited number of write types supported:
      * https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-storage-blob-output?tabs=csharp#usage
      * I'm using stream in the example below
      * This is the sdk sample instead of the output binding below in case you want to run this:
      * https://docs.microsoft.com/en-us/azure/storage/blobs/storage-upload-process-images?tabs=dotnet%2Cazure-powershell#upload-an-image
      */
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [Blob("test", System.IO.FileAccess.Write, Connection = "your app setting here")] CloudBlobContainer blobContainer,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            //name this what you want, i am just pulling this from the default function app template to pass in a name, can store a normal variable
            string blobName = req.Query["name"];

            //blob name inside the container, anything you want
            var cloudBlockBlob = blobContainer.GetBlockBlobReference(blobName);

            //data uploaded to the blob in the container
            await cloudBlockBlob.UploadTextAsync("example data");

            //some sample response output
            string responseMessage = "This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }
    }
}




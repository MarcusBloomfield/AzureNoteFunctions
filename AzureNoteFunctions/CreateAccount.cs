using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzureNoteFunctions
{
    public static class CreateAccount
    {
        [FunctionName("CreateAccount")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,ILogger log)
        {
            DataBaseConnector dataBaseConnector = new DataBaseConnector(log);
            log.LogInformation("creatAccount01");

            string userName = req.Query["userName"];
            string passWord = req.Query["passWord"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            userName = userName ?? data?.userName;
            passWord = passWord ?? data?.passWord;

            log.LogInformation("creatAccount02");
            bool IsCreated = dataBaseConnector.TryCreateAccount(userName, passWord);

            log.LogInformation("creatAccount03");
            string responseMessage = IsCreated.ToString();

            log.LogInformation("creatAccount04");
            return new OkObjectResult(responseMessage);
        }
    }
}

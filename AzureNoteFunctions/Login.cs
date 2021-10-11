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
    public static class Login
    {
        [FunctionName("Login")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req, ILogger log)
        {
            DataBaseConnector dataBaseConnector = new DataBaseConnector(log);
            log.LogInformation("C# HTTP trigger function processed a request.");

            string userName = req.Query["loginUserName"];
            string passWord = req.Query["loginPassword"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            userName = userName ?? data?.loginUserName;
            passWord = passWord ?? data?.loginPassword;

            log.LogInformation("USERNAME LOGIN" + userName);
            log.LogInformation("PASSWORD LOGIN" + passWord);

            string responseMessage = dataBaseConnector.TryLogin(userName, passWord);

            log.LogInformation($"Login Response {responseMessage} ");

            return new OkObjectResult(responseMessage);
        }
    }
}

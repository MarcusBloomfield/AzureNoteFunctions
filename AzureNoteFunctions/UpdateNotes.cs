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
    public static class UpdateNotes
    {
        [FunctionName("UpdateNotes")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,ILogger log)
        {
            log.LogInformation("Start");
            DataBaseConnector dataBaseConnector = new DataBaseConnector(log);
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["userName"];
            string password = req.Query["passWord"];
            string note = req.Query["note"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.userName;
            password = password ?? data?.passWord;
            note = note ?? data?.note;

            bool worked = dataBaseConnector.AddUsersNotes(note, name, password);

            string responseMessage = worked.ToString();

            log.LogInformation("DATA : " + note);

            return new OkObjectResult(responseMessage);
        }
    }
}

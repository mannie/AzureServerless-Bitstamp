#r "Newtonsoft.Json"

using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

public static async Task<IActionResult> Run(HttpRequest req, ILogger log)
{
    log.LogInformation("C# HTTP trigger function processed a request.");

    string value = req.Query["timestamp"];

    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
    dynamic data = JsonConvert.DeserializeObject(requestBody);
    value = value ?? data?.timestamp;

    var timestamp = Convert.ToInt64(value);

    DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(timestamp);
    DateTime dateTime = dateTimeOffset.UtcDateTime;

    return dateTime != null
        ? (ActionResult)new OkObjectResult(dateTime)
        : new BadRequestObjectResult(null);
}

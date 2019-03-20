# Azure Serverless
Use Logic Apps, Functions, and API Management to query the Bitstamp REST API.

## Create Bitstamp API abstraction via Logic App
1. Create a Logic App in the portal; select the _Request-Response_ template for your Logic App.
1. Update the schema of the HTTP trigger's body using the following as a sample body:
   ```json
   { "ticker" : "btcusd" }
   ```
1. Add an HTTP action pointing to **https://www.bitstamp.net/api/v2/ticker/$ticker** where `$ticker` is the dynamic parameter from the previous step.
1. Parse the JSON response of the HTTP request (i.e. the result of the previous action).
1. Compose the output payload using the format
   ```json
   {
      "last" : $last ,
      "open" : $open,
      "timestamp" : $timestamp
   }
   ```
   where
    * `$last` is a dynamic parameter, converted to `float`;
    * `$open` is a dynamic parameter, converted to `float`;
    * `$timestamp` is a dynamic parameter of type `string`;

To test using other ticker symbols, refer to [Bitstamp](https://www.bitstamp.net) for the available options.

If you're looking to automate the deployment of the Logic App's JSON, verify that the content looks something like [LogicApp-API.json](LogicApp-API.json).

## Create Function for timestamp conversion
1. Update HTTP trigger Function with code from [Function-ConvertTimestamp.cs](Function-ConvertTimestamp.cs).
1. Test the function by making a `POST` request using the payload:
   ```json
   { "timestamp" : "1549991478" }
   ```
   The response should be a string similar to:
   ```
   2019-02-12T17:11:18Z
   ```

## Check a ticker's value on a schedule via Logic App
1. Add a recurring trigger to your Logic App.
1. Invoke the previously created Logic App, passing in your choice of value for _ticker_.  
1. Parse the JSON response returned from the Logic App invocation, using the sample response:
   ```json
   {
      "last" : 1.12962,
      "open" : 1.12713,
      "timestamp" : "1549992442"
   }
   ```
1. Invoke the `ConvertTimestamp` function, passing in timestamp returned from the previous action.
1. Create a Condition (flow control action), and use the results of prior steps as you wish.

If you're looking to automate the deployment of the Logic App's JSON, verify that the content looks something like [LogicApp-Recurring.json](LogicApp-Recurring.json).

## API Management
This snippet might come in handy if you're going to convert a `POST` to a `GET`:
```xml
<set-body>
    @{
        return new JObject(new JProperty("ticker", context.Request.MatchedParameters["ticker"].ToString())).ToString();
    }
</set-body>
```

# Serverless

## Create Bitstamp API abstraction via Logic App 
1. Use the following body as sample playload for the HTTP trigger: `{ "ticker" : "btcusd" }`.
1. Add an HTTP action pointing to **https://www.bitstamp.net/api/v2/ticker/$ticker** where _$ticker_ is a dynamic parameter. 
1. Parse the JSON response of the HTTP request (i.e. the result of the previous action).
1. Compose the output payload using the format **`{ "last": $last , "open": $open, "timestamp": $timestamp }`** where
    * _$last_ is a dynamic parameter, converted to `float`;
    * _$open_ is a dynamic parameter, converted to `float`;
    * _$timestamp_ is a dynamic parameter of type `string`;

To test using other ticker symbols, refer to [Bitstamp](https://www.bitstamp.net) for the available options.

If you're looking to automate the deployment of the Logic App's JSON, verify that the content looks something like [LogicApp-API.json](#file-LogicApp-API-json).

## Create Function for timestamp conversion
1. Update HTTP trigger Function with code from [Function-ConvertTimestamp.cs](#file-function-converttimestamp-cs).
1. Test the function by making a `POST` request using payload `{ "timestamp" : "1549991478" }`; the response should be a string containing `2019-02-12T17:11:18Z`.

## Check a ticker's value on a schedule via Logic App 
1. Add a recurring trigger to your Logic App.
1. Invoke the previously created Logic App, passing in your choice of value for _ticker_.  
1. Parse the JSON response returned from the Logic App invocation, using the sample response: `{ "last": 1.12962, "open": 1.12713, "timestamp": "1549992442" }`.
1. Invoke the `ConvertTimestamp` function, passing in timestamp returned from the previous action.
1. Create a Condition (flow control action), and use the results of prior steps as you wish.

If you're looking to automate the deployment of the Logic App's JSON, verify that the content looks something like [LogicApp-Recurring.json](#file-LogicApp-Recurring-json).

## API Management
This snippet might come in handy if you're going to convert a `POST` to a `GET`:
```
<set-body>
    @{
        return new JObject(new JProperty("ticker", context.Request.MatchedParameters["ticker"].ToString())).ToString();
    }
</set-body>
```
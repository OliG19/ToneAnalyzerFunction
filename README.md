# ToneAnalyzerFunction

The Tone Analyzer function takes advantage of the  [IBM Watson Tone Anaylyzer API](https://www.ibm.com/watson/services/tone-analyzer/).

To run this project locally from visual studio you will need to install the [Azure function tools for visual studio](https://docs.microsoft.com/en-us/azure/azure-functions/functions-develop-vs).
You will then need to apply the below settings to your local.settings.json file so configuration can be binded correctly;
<br>
`{`<br>
 &nbsp; `"WatsonApiKey": "99HcssymdFkzvsPxb6ti5RpxKBQsGhqo5teo9kUjxhh7",`<br>
 &nbsp; `"WatsonUrl": "https://gateway-lon.watsonplatform.net/tone-analyzer/api/v3/tone?version=2017-09-21",`<br>
 &nbsp; `"JokeUrl": "https://dad-jokes.p.rapidapi.com/random/joke",`<br>
 &nbsp;`"JokeApikey": "576846c5b2msh69c8ca92424bc2dp1ee2b7jsn52ca18093f1d",`<br>
 &nbsp;`"JokeApiHost": "dad-jokes.p.rapidapi.com",`<br>   &nbsp;`"CosmosDBConnectionString":"AccountEndpoint=https://tonelyzer.documents.azure.com:443/;AccountKey=D5FioNph9nFwXASyHwcx3Jx4sY7aRPKNAwnDNjgqRisqydQVtCQDGzhns4paNZIhbYUqkivdrz7Mk8vpOlBL6A==;"`<br>
`}`<br>

Once installed you can simply run the project and then invoke the function from Postman. The function consumes a RESTful POST request, with a body json format of:<br>
`
{
    "text": ""
}
`<br>
your text will be analyzed by Watson and a tone(s) will be produced which best matches that of the content. Tones are given a score (double) in the range of 0.5 to 1. A score greater than 0.75 indicates a high likelihood that the tone is perceived in the content.

This Azure function consumes the list of tones produced by Watson, determines which tone is most apparent and saves this tone to a CosmosDB instance hosted in Azure. Each tone is saved along with;
* The tone score
* The original text/comment within the request
* A Guid as its SKU

If the tone is perceived as not a cheerful or happy tone, a joke from the [Rapid Joke API](https://rapidapi.com/webknox/api/jokes) is appended to the Final Tone object which we save to the database. 

## Responses
* The Final Tone object is also returned in the response along with a 200 OK status code.
* 400 Bad Request status code is returned when a request is made without a valid json body.
* 500 Internal Server Error is returned and a HttpRequestException caught when either a tone or joke can not be consumed by either underlying service.

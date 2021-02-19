# ToneAnalyzerFunction

The Tone Analyzer function takes advantage of the  [IBM Watson Tone Anaylyzer API](https://www.ibm.com/watson/services/tone-analyzer/).

By sending a piece of text to this Azure Function in this json format;
`
{
    "text": ""
}
`
your text will be analyzed by Watson and a tone(s) will be produced which best match that of the content. Tones are given a score (double) in the range of 0.5 to 1. A score greater than 0.75 indicates a high likelihood that the tone is perceived in the content.

This Azure function consumes the list of tones produced by Watson, determines which tone is most apparent and saves this tone to a CosmosDB instance hosted in Azure. Each tone is saved along with;
* The tone score
* The original text/comment within the request
* A Guid as its SKU

If the tone is perceived as not a cheerful or happy tone, a joke from the [Rapid Joke API](https://rapidapi.com/webknox/api/jokes) is appended to the Final Tone object which we save to the database. 

## Responses
The Final Tone object is also returned in the response along with a 200 OK status code.
A 400 Bad Request status code is returned when a request is made without a valid json body.
A 500 Internal Server Error is returned and a HttpRequestException caught when either a tone or joke can not be consumed by either underlying service.

# AI Image and Text Generator

ASP.NET Core (Razor Pages) web app. Generates an image (MAI-Image-2.6) and text (gpt-5) from a user prompt using Microsoft Foundry. API requests are asynchronous (async/await, HttpClient).

## Run

The endpoint and API key are stored in user-secrets:

```
dotnet user-secrets set "Foundry:Endpoint" "<endpoint>"
dotnet user-secrets set "Foundry:ApiKey" "<api-key>"
dotnet user-secrets set "Foundry:ImageModel" "MAI-Image-2.6"
dotnet user-secrets set "Foundry:TextModel" "gpt-5"
dotnet run
```

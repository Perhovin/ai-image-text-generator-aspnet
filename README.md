# AI Image and Text Generator

ASP.NET Core Razor Pages web app that generates an image (MAI-Image-2.6) and text (gpt-5) from a user prompt using Microsoft Foundry. Course task: asynchronous programming (async/await, HttpClient).

## Run

1. Set secrets:

```
dotnet user-secrets set "Foundry:Endpoint" "<endpoint>"
dotnet user-secrets set "Foundry:ApiKey" "<api-key>"
dotnet user-secrets set "Foundry:ImageModel" "MAI-Image-2.6"
dotnet user-secrets set "Foundry:TextModel" "gpt-5"
```

2. Start the app:

```
dotnet run
```

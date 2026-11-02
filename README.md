# AI Image and Text Generator

Веб-приложение на ASP.NET Core (Razor Pages). По описанию пользователя генерирует изображение (MAI-Image-2.6) и текст (gpt-5) через Microsoft Foundry. Запросы к API асинхронные (async/await, HttpClient).

## Запуск

Ключ и адрес хранятся в user-secrets:

```
dotnet user-secrets set "Foundry:Endpoint" "<endpoint>"
dotnet user-secrets set "Foundry:ApiKey" "<api-key>"
dotnet user-secrets set "Foundry:ImageModel" "MAI-Image-2.6"
dotnet user-secrets set "Foundry:TextModel" "gpt-5"
dotnet run
```

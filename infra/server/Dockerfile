FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY src/presenters/shnotes.webapi/*.csproj ./shnotes.webapi/
RUN dotnet restore ./shnotes.webapi/ShNotes.WebApi.csproj

COPY src/presenters/shnotes.webapi/ ./shnotes.webapi/

WORKDIR /src/shnotes.webapi
RUN dotnet publish ShNotes.WebApi.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "ShNotes.WebApi.dll"]
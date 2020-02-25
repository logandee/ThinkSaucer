FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build-env

WORKDIR /app

COPY BrightIdea.csproj .
RUN dotnet restore

COPY . .
RUN dotnet ef migrations add FirstMigration
RUN dotnet ef database update

RUN dotnet publish -c Relase -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2

WORKDIR /app
COPY --from=build-env /app/out .

ENTRYPOINT ["dotnet", "BrightIdea.dll"]
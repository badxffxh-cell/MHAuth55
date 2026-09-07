FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["AuthApi.csproj", "./"]
RUN dotnet restore "AuthApi.csproj"
COPY . .
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://*:
EXPOSE 8080
ENTRYPOINT ["dotnet", "AuthApi.dll"]

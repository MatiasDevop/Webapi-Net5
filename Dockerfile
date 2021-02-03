#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["AppServiceNet5.csproj", ""]
RUN dotnet restore "./AppServiceNet5.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "AppServiceNet5.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AppServiceNet5.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AppServiceNet5.dll"]
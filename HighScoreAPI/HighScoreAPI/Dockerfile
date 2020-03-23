#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["HighScoreAPI/HighScoreAPI.csproj", "HighScoreAPI/"]
RUN dotnet restore "HighScoreAPI/HighScoreAPI.csproj"
COPY . .
WORKDIR "/src/HighScoreAPI"
RUN dotnet build "HighScoreAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HighScoreAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HighScoreAPI.dll"]
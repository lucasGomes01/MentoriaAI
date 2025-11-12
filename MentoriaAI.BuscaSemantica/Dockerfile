FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["MentoriaAI.Contracts/MentoriaAI.Contracts.csproj", "MentoriaAI.Contracts/"]
COPY ["MentoriaAI.BuscaSemantica/MentoriaAI.BuscaSemantica.csproj", "MentoriaAI.BuscaSemantica/"]

RUN dotnet restore "MentoriaAI.BuscaSemantica/MentoriaAI.BuscaSemantica.csproj"

COPY . .
WORKDIR "/src/MentoriaAI.BuscaSemantica"

RUN dotnet build "MentoriaAI.BuscaSemantica.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MentoriaAI.BuscaSemantica.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "MentoriaAI.BuscaSemantica.dll"]

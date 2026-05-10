# Base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copia Contracts e Cadastro
COPY ["MentoriaAI.Contracts/MentoriaAI.Contracts.csproj", "MentoriaAI.Contracts/"]
COPY ["MentoriaAI.Cadastro/MentoriaAI.Cadastro.csproj", "MentoriaAI.Cadastro/"]

# Restaura dependências
RUN dotnet restore "MentoriaAI.Cadastro/MentoriaAI.Cadastro.csproj"

# Copia tudo
COPY . .
WORKDIR "/src/MentoriaAI.Cadastro"

# Build
RUN dotnet build "MentoriaAI.Cadastro.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MentoriaAI.Cadastro.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MentoriaAI.Cadastro.dll"]

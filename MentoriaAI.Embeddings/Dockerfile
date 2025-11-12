# Base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copia o projeto Contracts e Embeddings
COPY ["MentoriaAI.Contracts/MentoriaAI.Contracts.csproj", "MentoriaAI.Contracts/"]
COPY ["MentoriaAI.Embeddings/MentoriaAI.Embeddings.csproj", "MentoriaAI.Embeddings/"]

# Restaura dependências
RUN dotnet restore "MentoriaAI.Embeddings/MentoriaAI.Embeddings.csproj"

# Copia tudo
COPY . .
WORKDIR "/src/MentoriaAI.Embeddings"

# Compila
RUN dotnet build "MentoriaAI.Embeddings.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publica
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MentoriaAI.Embeddings.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MentoriaAI.Embeddings.dll"]

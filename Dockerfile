# Imagen base para producción
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Imagen para build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiamos todos los proyectos
COPY ["API_PensamientoAlternativo/API_PensamientoAlternativo.csproj", "API_PensamientoAlternativo/"]
COPY ["PensamientoAlternativo.Application/PensamientoAlternativo.Application.csproj", "PensamientoAlternativo.Application/"]
COPY ["PensamientoAlternativo.Domain/PensamientoAlternativo.Domain.csproj", "PensamientoAlternativo.Domain/"]
COPY ["PensamientoAlternativo.Infrastructure/PensamientoAlternativo.Infrastructure.csproj", "PensamientoAlternativo.Infrastructure/"]
COPY ["PensamientoAlternativo.Persistance/PensamientoAlternativo.Persistance.csproj", "PensamientoAlternativo.Persistance/"]

# Restauramos dependencias
RUN dotnet restore "API_PensamientoAlternativo/API_PensamientoAlternativo.csproj"

# Copiamos el resto del código
COPY . .

# Publicamos la aplicación
WORKDIR "/src/API_PensamientoAlternativo"
RUN dotnet publish "API_PensamientoAlternativo.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Imagen final
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "API_PensamientoAlternativo.dll"]

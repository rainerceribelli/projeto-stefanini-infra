# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY GestaoDeCadastro.Crosscutting/*.csproj ./GestaoDeCadastro.Crosscutting/
COPY GestaoDeCadastro.Domain/*.csproj ./GestaoDeCadastro.Domain/
COPY GestaoDeCadastro.Infraestructure/*.csproj ./GestaoDeCadastro.Infraestructure/
COPY GestaoDeCadastro.Interface/*.csproj ./GestaoDeCadastro.Interface/
COPY GestaoDeCadastro.Services/*.csproj ./GestaoDeCadastro.Services/

# Restaurar pacotes
RUN dotnet restore GestaoDeCadastro.Interface/GestaoDeCadastro.Interface.csproj

# Copiar código fonte
COPY GestaoDeCadastro.Crosscutting/. ./GestaoDeCadastro.Crosscutting/
COPY GestaoDeCadastro.Domain/. ./GestaoDeCadastro.Domain/
COPY GestaoDeCadastro.Infraestructure/. ./GestaoDeCadastro.Infraestructure/
COPY GestaoDeCadastro.Interface/. ./GestaoDeCadastro.Interface/
COPY GestaoDeCadastro.Services/. ./GestaoDeCadastro.Services/

# Publicar
RUN dotnet publish GestaoDeCadastro.Interface/GestaoDeCadastro.Interface.csproj -c Release -o /app

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .

# ⚠️ Escutar na porta 80
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

ENTRYPOINT ["dotnet", "GestaoDeCadastro.Interface.dll"]

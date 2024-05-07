FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /
EXPOSE 80

# Copia a solução AlluRider.sln
COPY AlluRider.sln .

# Copia os arquivos de projeto
COPY services/user/User.API/User.API.csproj /services/user/User.API/
COPY services/user/User.Domain/User.Domain.csproj /services/user/User.Domain/
COPY services/user/User.Infra/User.Infra.csproj /services/user/User.Infra/
COPY services/user/User.Services/User.Services.csproj /services/user/User.Services/
COPY services/user/User.S3.Lib/User.S3.Lib.csproj /services/user/User.S3.Lib/

# Restore package deps
RUN dotnet restore AlluRider.sln

# Copia os diretórios de cada projeto para dentro de /services/user
COPY services/user/User.API /services/user/User.API
COPY services/user/User.Domain /services/user/User.Domain
COPY services/user/User.Infra /services/user/User.Infra
COPY services/user/User.Services /services/user/User.Services
COPY services/user/User.S3.Lib /services/user/User.S3.Lib


WORKDIR "/services/user/User.API"
RUN dotnet build "./User.API.csproj" -c Release -o /app/src/out

#Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/src/out .
ENTRYPOINT [ "dotnet",  "User.API.dll"]
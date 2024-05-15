FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /
EXPOSE 80

# Copia a solução AlluRider.sln
COPY AlluRider.sln .

# Copia os arquivos de projeto Order
COPY services/order/Order.API/Order.API.csproj /services/order/Order.API/
COPY services/order/Order.Domain/Order.Domain.csproj /services/order/Order.Domain/
COPY services/order/Order.Infra/Order.Infra.csproj /services/order/Order.Infra/
COPY services/order/Order.Services/Order.Services.csproj	/services/order/Order.Services/
COPY services/rabbitmq/RabbitMq.Notify/RabbitMq.Notify.csproj /services/rabbitmq/RabbitMq.Notify/

# Copia os arquivos de projeto Vehicle
COPY services/vehicle/Vehicle.API/Vehicle.API.csproj /services/vehicle/Vehicle.API/
COPY services/vehicle/Vehicle.Domain/Vehicle.Domain.csproj /services/vehicle/Vehicle.Domain/
COPY services/vehicle/Vehicle.Infra/Vehicle.Infra.csproj /services/vehicle/Vehicle.Infra/
COPY services/vehicle/Vehicle.Services/Vehicle.Services.csproj /services/vehicle/Vehicle.Services/
COPY services/rabbitmq/RabbitMq.Notify/RabbitMq.Notify.csproj /services/rabbitmq/RabbitMq.Notify/

# Copia os arquivos de projeto User
COPY services/user/User.API/User.API.csproj /services/user/User.API/
COPY services/user/User.Domain/User.Domain.csproj /services/user/User.Domain/
COPY services/user/User.Infra/User.Infra.csproj /services/user/User.Infra/
COPY services/user/User.Services/User.Services.csproj /services/user/User.Services/
COPY services/user/User.S3.Lib/User.S3.Lib.csproj /services/user/User.S3.Lib/
COPY services/rabbitmq/RabbitMq.Notify/RabbitMq.Notify.csproj /services/rabbitmq/RabbitMq.Notify/


# Restore package deps
RUN dotnet restore AlluRider.sln

# Copia os diretórios de cada projeto para dentro de /services/order
COPY services/order/Order.API/ /services/order/Order.API/
COPY services/order/Order.Domain/ /services/order/Order.Domain
COPY services/order/Order.Infra /services/order/Order.Infra
COPY services/order/Order.Services /services/order/Order.Services
COPY services/rabbitmq/RabbitMq.Notify /services/rabbitmq/RabbitMq.Notify


WORKDIR "/services/order/Order.API"
RUN dotnet build "./Order.API.csproj" -c Release -o /app/src/out

#Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/src/out .
ENTRYPOINT [ "dotnet",  "Order.API.dll"]
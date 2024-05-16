FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /
EXPOSE 80

# Copia a solução AlluRider.sln
COPY AlluRider.sln .

# Copia os arquivos de projeto Notify
COPY services/notify/Notify.API/Notify.API.csproj /services/notify/Notify.API/
COPY services/notify/Notify.Domain/Notify.Domain.csproj /services/notify/Notify.Domain/
COPY services/notify/Notify.Infra/Notify.Infra.csproj /services/notify/Notify.Infra/
COPY services/notify/Notify.Services/Notify.Services.csproj	/services/notify/Notify.Services/
COPY services/awsbroker/AWS.Notify/AWS.Notify.csproj /services/awsbroker/AWS.Notify/

# Copia os arquivos de projeto Order
COPY services/order/Order.API/Order.API.csproj /services/order/Order.API/
COPY services/order/Order.Domain/Order.Domain.csproj /services/order/Order.Domain/
COPY services/order/Order.Infra/Order.Infra.csproj /services/order/Order.Infra/
COPY services/order/Order.Services/Order.Services.csproj /services/order/Order.Services/
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

# Copia os arquivos de projeto TestUser
COPY test/user/UserTest.csproj /test/user/

# Restore package deps
RUN dotnet restore AlluRider.sln

# Copia os diretórios de cada projeto para dentro de /services/order
COPY services/notify/Notify.API/ /services/notify/Notify.API/ 
COPY services/notify/Notify.Domain/ /services/notify/Notify.Domain/
COPY services/notify/Notify.Infra/ /services/notify/Notify.Infra
COPY services/notify/Notify.Services/ /services/notify/Notify.Services
COPY services/awsbroker/AWS.Notify/ /services/awsbroker/AWS.Notify/


WORKDIR "/services/notify/Notify.API"
RUN dotnet build "./Notify.API.csproj" -c Release -o /app/src/out

#Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/src/out .
ENTRYPOINT [ "dotnet",  "Notify.API.dll"]
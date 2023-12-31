#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Presentation/ECommerceMVC/ECommerceMVC.csproj", "Presentation/ECommerceMVC/"]
COPY ["Core/ECommerce.Application/ECommerce.Application.csproj", "Core/ECommerce.Application/"]
COPY ["Core/ECommerce.Domain/ECommerce.Domain.csproj", "Core/ECommerce.Domain/"]
COPY ["Infrastructure/ECommerce.Infrastucture/ECommerce.Infrastucture.csproj", "Infrastructure/ECommerce.Infrastucture/"]
COPY ["Infrastructure/ECommerce.Persistence/ECommerce.Persistence.csproj", "Infrastructure/ECommerce.Persistence/"]
RUN dotnet restore "Presentation/ECommerceMVC/ECommerceMVC.csproj"
COPY . .
WORKDIR "/src/Presentation/ECommerceMVC"
RUN dotnet build "ECommerceMVC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ECommerceMVC.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ECommerceMVC.dll"]
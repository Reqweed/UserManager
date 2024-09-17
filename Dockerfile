FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app 
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["UserManager.API/UserManager.API.csproj", "UserManager.API/"]
RUN dotnet restore "UserManager.API/UserManager.API.csproj"
COPY . . 
WORKDIR "/src/GroceryShop.API"
RUN dotnet build "./UserManager.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UserManager.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserManager.API.dll"]
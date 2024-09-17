FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app 
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["UserManager/UserManager.csproj", "UserManager/"]
RUN dotnet restore "UserManager/UserManager.csproj"
COPY . . 
WORKDIR "/src/GroceryShop"
RUN dotnet build "./UserManager.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UserManager.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserManager.dll"]
#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Apis/Apis.csproj", "Apis/"]
COPY ["Datas/Datas.csproj", "Datas/"]
COPY ["Helpers/Helpers.csproj", "Helpers/"]
COPY ["Dtos/Dtos.csproj", "Dtos/"]
COPY ["Constants/Constants.csproj", "Constants/"]
COPY ["Settings/Settings.csproj", "Settings/"]
COPY ["Repositories/Repositories.csproj", "Repositories/"]
RUN dotnet restore "Apis/Apis.csproj"
COPY . .
WORKDIR "/src/Apis"
RUN dotnet build "Apis.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Apis.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Apis.dll"]
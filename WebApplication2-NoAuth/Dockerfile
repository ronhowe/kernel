#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["WebApplication2-NoAuth/WebApplication2-NoAuth.csproj", "WebApplication2-NoAuth/"]
RUN dotnet restore "WebApplication2-NoAuth/WebApplication2-NoAuth.csproj"
COPY . .
WORKDIR "/src/WebApplication2-NoAuth"
RUN dotnet build "WebApplication2-NoAuth.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebApplication2-NoAuth.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApplication2-NoAuth.dll"]

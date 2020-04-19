#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/Monitor/GeekTime.HealthChecksHost/GeekTime.HealthChecksHost.csproj", "src/Monitor/GeekTime.HealthChecksHost/"]
COPY ["NuGet.config","NuGet.config"]
RUN dotnet restore "src/Monitor/GeekTime.HealthChecksHost/GeekTime.HealthChecksHost.csproj"
COPY . .
WORKDIR "/src/src/Monitor/GeekTime.HealthChecksHost"
RUN dotnet build "GeekTime.HealthChecksHost.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GeekTime.HealthChecksHost.csproj" -c Release -o /app/publish

FROM base AS final
# RUN ln -sf /usr/share/zoneinfo/Asia/Shanghai /etc/localtime  #需要时设置时区
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GeekTime.HealthChecksHost.dll"]
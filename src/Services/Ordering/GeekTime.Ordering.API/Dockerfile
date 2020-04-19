#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 81
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/Services/Ordering/GeekTime.Ordering.API/GeekTime.Ordering.API.csproj", "src/Services/Ordering/GeekTime.Ordering.API/"]
COPY ["src/Services/Ordering/GeekTime.Ordering.Infrastructure/GeekTime.Ordering.Infrastructure.csproj", "src/Services/Ordering/GeekTime.Ordering.Infrastructure/"]
COPY ["src/Services/Ordering/GeekTime.Ordering.Domain/GeekTime.Ordering.Domain.csproj", "src/Services/Ordering/GeekTime.Ordering.Domain/"]
COPY ["src/Shared/GeekTime.Domain.Abstractions/GeekTime.Domain.Abstractions.csproj", "src/Shared/GeekTime.Domain.Abstractions/"]
COPY ["src/Shared/GeekTime.Infrastructure.Core/GeekTime.Infrastructure.Core.csproj", "src/Shared/GeekTime.Infrastructure.Core/"]
COPY ["NuGet.config","NuGet.config"]
RUN dotnet restore "src/Services/Ordering/GeekTime.Ordering.API/GeekTime.Ordering.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Ordering/GeekTime.Ordering.API"
RUN dotnet build "GeekTime.Ordering.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GeekTime.Ordering.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
# RUN ln -sf /usr/share/zoneinfo/Asia/Shanghai /etc/localtime  #需要时设置时区
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GeekTime.Ordering.API.dll"]
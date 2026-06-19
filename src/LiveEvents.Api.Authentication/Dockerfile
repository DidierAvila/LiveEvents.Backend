FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/LiveEvents.Api.Authentication/LiveEvents.Api.Authentication.csproj", "src/LiveEvents.Api.Authentication/"]
COPY ["src/LiveEvents.Api.Authentication.Application/LiveEvents.Api.Authentication.Application.csproj", "src/LiveEvents.Api.Authentication.Application/"]
COPY ["src/LiveEvents.Api.Common/LiveEvents.Api.Common.csproj", "src/LiveEvents.Api.Common/"]
COPY ["src/LiveEvents.Api.Domain/LiveEvents.Api.Domain.csproj", "src/LiveEvents.Api.Domain/"]
COPY ["src/LiveEvents.Api.Infrastructure/LiveEvents.Api.Infrastructure.csproj", "src/LiveEvents.Api.Infrastructure/"]

RUN dotnet restore "src/LiveEvents.Api.Authentication/LiveEvents.Api.Authentication.csproj"

COPY . .
WORKDIR "/src/src/LiveEvents.Api.Authentication"
RUN dotnet publish "LiveEvents.Api.Authentication.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "LiveEvents.Api.Authentication.dll"]

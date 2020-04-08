#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /sln
COPY ["src/Pragmatest.Wallets.Web/Pragmatest.Wallets.Web.csproj", "sln/Pragmatest.Wallets.Web/"]

RUN dotnet restore "sln/Pragmatest.Wallets.Web/Pragmatest.Wallets.Web.csproj"
COPY . .
WORKDIR "/sln/src/Pragmatest.Wallets.Web"
RUN dotnet build "Pragmatest.Wallets.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Pragmatest.Wallets.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pragmatest.Wallets.Web.dll"]
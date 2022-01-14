FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src

COPY ["TLPokemon/TLPokemon.WebApi/TLPokemon.WebApi.csproj", "TLPokemon/TLPokemon.WebApi/"]

RUN dotnet restore "TLPokemon/TLPokemon.WebApi/TLPokemon.WebApi.csproj"
COPY . .
WORKDIR "/src/TLPokemon/TLPokemon.WebApi"
RUN dotnet build "TLPokemon.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TLPokemon.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TLPokemon.WebApi.dll"]


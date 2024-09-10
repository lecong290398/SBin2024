#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/DTKH2024.SbinSolution.Migrator/DTKH2024.SbinSolution.Migrator.csproj", "src/DTKH2024.SbinSolution.Migrator/"]
COPY ["src/DTKH2024.SbinSolution.EntityFrameworkCore/DTKH2024.SbinSolution.EntityFrameworkCore.csproj", "src/DTKH2024.SbinSolution.EntityFrameworkCore/"]
COPY ["src/DTKH2024.SbinSolution.Core/DTKH2024.SbinSolution.Core.csproj", "src/DTKH2024.SbinSolution.Core/"]
COPY ["src/DTKH2024.SbinSolution.Core.Shared/DTKH2024.SbinSolution.Core.Shared.csproj", "src/DTKH2024.SbinSolution.Core.Shared/"]
RUN dotnet restore "src/DTKH2024.SbinSolution.Migrator/DTKH2024.SbinSolution.Migrator.csproj"
COPY . .
WORKDIR "/src/src/DTKH2024.SbinSolution.Migrator"
RUN dotnet build "DTKH2024.SbinSolution.Migrator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DTKH2024.SbinSolution.Migrator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DTKH2024.SbinSolution.Migrator.dll", "-s"]

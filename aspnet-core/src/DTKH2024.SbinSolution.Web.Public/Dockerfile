#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/DTKH2024.SbinSolution.Web.Public/DTKH2024.SbinSolution.Web.Public.csproj", "src/DTKH2024.SbinSolution.Web.Public/"]
COPY ["src/DTKH2024.SbinSolution.Web.Core/DTKH2024.SbinSolution.Web.Core.csproj", "src/DTKH2024.SbinSolution.Web.Core/"]
COPY ["src/DTKH2024.SbinSolution.Application/DTKH2024.SbinSolution.Application.csproj", "src/DTKH2024.SbinSolution.Application/"]
COPY ["src/DTKH2024.SbinSolution.Application.Shared/DTKH2024.SbinSolution.Application.Shared.csproj", "src/DTKH2024.SbinSolution.Application.Shared/"]
COPY ["src/DTKH2024.SbinSolution.Core.Shared/DTKH2024.SbinSolution.Core.Shared.csproj", "src/DTKH2024.SbinSolution.Core.Shared/"]
COPY ["src/DTKH2024.SbinSolution.Core/DTKH2024.SbinSolution.Core.csproj", "src/DTKH2024.SbinSolution.Core/"]
COPY ["src/DTKH2024.SbinSolution.EntityFrameworkCore/DTKH2024.SbinSolution.EntityFrameworkCore.csproj", "src/DTKH2024.SbinSolution.EntityFrameworkCore/"]
COPY ["src/DTKH2024.SbinSolution.GraphQL/DTKH2024.SbinSolution.GraphQL.csproj", "src/DTKH2024.SbinSolution.GraphQL/"]
RUN dotnet restore "src/DTKH2024.SbinSolution.Web.Public/DTKH2024.SbinSolution.Web.Public.csproj"
COPY . .
WORKDIR "/src/src/DTKH2024.SbinSolution.Web.Public"
RUN dotnet build "DTKH2024.SbinSolution.Web.Public.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DTKH2024.SbinSolution.Web.Public.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DTKH2024.SbinSolution.Web.Public.dll"]

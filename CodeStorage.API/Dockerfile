FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal AS base
WORKDIR /app
EXPOSE 5070

ENV ASPNETCORE_URLS=http://+:5070

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /src
COPY ["CodeStorage.API/CodeStorage.API.csproj", "CodeStorage.API/"]
RUN dotnet restore "CodeStorage.API/CodeStorage.API.csproj"
COPY . .
WORKDIR "/src/CodeStorage.API"
RUN dotnet build "CodeStorage.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CodeStorage.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CodeStorage.API.dll"]

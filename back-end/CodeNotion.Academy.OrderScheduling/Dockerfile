FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["CodeNotion.Academy.OrderScheduling/CodeNotion.Academy.OrderScheduling.csproj", "CodeNotion.Academy.OrderScheduling/"]
RUN dotnet restore "CodeNotion.Academy.OrderScheduling/CodeNotion.Academy.OrderScheduling.csproj"
COPY . .
WORKDIR "/src/CodeNotion.Academy.OrderScheduling"
RUN dotnet build "CodeNotion.Academy.OrderScheduling.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CodeNotion.Academy.OrderScheduling.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CodeNotion.Academy.OrderScheduling.dll"]

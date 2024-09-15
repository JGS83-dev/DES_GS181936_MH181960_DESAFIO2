FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src

COPY *.csproj release/
WORKDIR /src/release
RUN dotnet restore 

COPY . release/
RUN dotnet publish -c Release -o /out

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS runtime
WORKDIR /app
COPY --from=build /out .

EXPOSE 5000

ENTRYPOINT ["dotnet", "APIGateway.dll"]
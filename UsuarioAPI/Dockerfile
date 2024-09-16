FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src

EXPOSE 443
EXPOSE 80
EXPOSE 7116
EXPOSE 5000

COPY *.csproj release/
WORKDIR /src/release
RUN dotnet restore 

COPY . release/
RUN dotnet publish -c Release -o /out

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS runtime
WORKDIR /app
COPY --from=build /out .

EXPOSE 7116

ENTRYPOINT ["dotnet", "UsuarioAPI.dll"]

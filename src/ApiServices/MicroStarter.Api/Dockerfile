#region (build_dev)
FROM microsoft/dotnet:2.1-sdk-alpine AS build
# Set the working directory witin the container
WORKDIR /src

# Copy all of the source files
COPY * ./

# Restore all packages
RUN dotnet restore ./MicroStarter.Api.csproj

# Build the source code
RUN dotnet build ./MicroStarter.Api.csproj

#& region (database)
# Ensure that we generate and migrate the database
RUN chmod +x migrations.sh
RUN chmod +x updatedb.sh
RUN sh migrations.sh
RUN sh updatedb.sh
#& end (database)

# Publish application
WORKDIR ..
RUN dotnet publish ./src/MicroStarter.Api.csproj --output "/dist"

# Build runtime image
FROM microsoft/dotnet:2.1-aspnetcore-runtime-alpine AS app
WORKDIR /app
COPY --from=build /dist .
ENV ASPNETCORE_URLS http://+:8000
ENV ASPNETCORE_ENVIRONMENT Docker_Production

ENTRYPOINT ["dotnet", "MicroStarter.Api.dll"]

#end (build_dev)
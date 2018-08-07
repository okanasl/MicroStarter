#region (build_dev)
FROM microsoft/dotnet:2.1-sdk-alpine AS build
# Set the working directory within the container
WORKDIR /src

# Copy all of the source files
COPY * ./
# Restore all packages
RUN dotnet restore ./Host/Host.csproj 

# Build the source code
RUN dotnet build ./Host/Host.csproj

# Ensure that we generate and migrate the database
RUN chmod +x migrations.sh
RUN chmod +x updatedb.sh
RUN sh migrations.sh
RUN sh updatedb.sh
# Publish application
WORKDIR ..
RUN dotnet publish ./src/Host/Host.csproj --output "/dist"

# Build runtime image
FROM microsoft/dotnet:2.1-aspnetcore-runtime-alpine AS app
WORKDIR /app
COPY --from=build /dist .
ENV ASPNETCORE_URLS http://+:5000
ENV ASPNETCORE_ENVIRONMENT Docker_Production
ENTRYPOINT ["dotnet", "Host.dll", "/seed"]

#endregion
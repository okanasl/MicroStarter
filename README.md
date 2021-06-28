Angular .Net Core IdentityServer Starter
---------------

## IMPORTANT 

You should not use this code in production since implicit flow is depreciated. Check new PKCE flow. But since they're similiar you can follow implementation in this codebase.

<b>IdentityServer4, .NET Core API, Angular Universal (SSR) Starter with cookie authentication, docker, nginx and redis support</b>
<p>Generated with <a href="https://github.com/DooMachine/MicroBoiler">MicroBoiler</a></p>

Getting started
---------------


# Clone the repository
git clone git@github.com:DooMachine/MicroStarter.git
cd MicroBoiler

# Set your ASPNETCORE_ENVIRONMENT environment to Development
# Remove this git config
rm -rf .git 
# Start Your PostgreSql and Rabbitmq
# If you dont have them:
docker-compose -f docker-compose-tools.yml up
# Your redis, postgresql and rabbitmq instances will start with username: doom, password: machine with default ports. You can access them with localhost.
# Install dependencies and configure
# Api Service
cd ApiServices/MicroStarter.Api/src
# Check your rabbitmq and postgresql username and password
dotnet restore
# Run Migrations
dotnet run
# IdentityServer4
cd IdentityServices/MicroStarter.Identity/src/Host
# Check your rabbitmq and postgresql username and password
dotnet restore
# Run Migrations (bash migrations.dev.sh)
dotnet run /seed #seed data for is4 configuration
# SSR Client
cd Clients/MicroStarter.AngularSsrClient
npm install
ng serve



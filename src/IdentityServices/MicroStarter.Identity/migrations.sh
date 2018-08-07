#!/bin/sh
cd Host
rm -rf Migrations

dotnet ef migrations add Grants -c PersistedGrantDbContext -o Migrations/IdentityServer/PersistedGrantDb
dotnet ef migrations add Config -c ConfigurationDbContext -o Migrations/IdentityServer/ConfigurationDb
dotnet ef migrations add Users -c UserDbContext -o Migrations/IdentityServer/UsersDb
dotnet ef migrations script -c PersistedGrantDbContext -o Migrations/IdentityServer/PersistedGrantDb.sql
dotnet ef migrations script -c ConfigurationDbContext -o Migrations/IdentityServer/ConfigurationDb.sql
dotnet ef migrations script -c UserDbContext -o Migrations/IdentityServer/UsersDb.sql
cd ..

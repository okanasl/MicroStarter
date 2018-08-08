#!/bin/sh
cd src
rm -rf Migrations

dotnet ef migrations add InitialMigrations -c MicroStarterApiContext -o Migrations/MicroStarter.Api/UsersDb
dotnet ef migrations script -c MicroStarterApiContext -o Migrations/MicroStarter.Api/MicroStarterApiContext.sql
cd ..

#!/bin/sh
cd src/Host
dotnet ef database update -c PersistedGrantDbContext
dotnet ef database update -c ConfigurationDbContext
dotnet ef database update -c UserDbContext
cd ../..

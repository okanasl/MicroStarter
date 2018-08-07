#!/bin/sh
cd src/Host
dotnet ef database drop -c PersistedGrantDbContext
dotnet ef database drop -c UserDbContext
cd ../..

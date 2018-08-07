#!/bin/sh
cd Host
dotnet ef database drop -c PersistedGrantDbContext
dotnet ef database drop -c UserDbContext
cd ..

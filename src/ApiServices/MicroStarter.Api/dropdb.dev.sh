#!/bin/sh
cd src
dotnet ef database drop -c MicroStarterApiContext
cd ..
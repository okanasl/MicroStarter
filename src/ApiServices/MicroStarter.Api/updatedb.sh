#!/bin/sh
cd src/MicroStarter.Api
dotnet ef database update -c MicroStarterApiContext
cd ../..

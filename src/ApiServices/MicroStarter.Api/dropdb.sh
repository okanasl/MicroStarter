#!/bin/sh
cd src/MicroStarter.Api
dotnet ef database drop -c MicroStarterApiContext
cd ../..

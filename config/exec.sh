#!/bin/bash
if [ $1 = "test" ]; then
	cd /home/
	dotnet test
else
	cd /home/net-platform/
	dotnet build
	/usr/share/dotnet/dotnet /home/net-platform/bin/Debug/netcoreapp2.2/net-platform.dll
fi
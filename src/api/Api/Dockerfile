FROM microsoft/aspnetcore:2.0

WORKDIR /app

COPY ./bin/Release/netcoreapp2.0/publish .
COPY ./libraries ./libraries

CMD ASPNETCORE_URLS=http://*:$PORT dotnet Api.dll
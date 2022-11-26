# docker build -t lotussocialmedia/datingapp .
#docker run --rm -t -it -p 8080:80 lotussocialmedia/datingapp:latest

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS buils-env
WORKDIR /app
EXPOSE 8080

# copy csproj and restore as distinct
COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Relesease -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0 
WORKDIR /app
COPY --from=buils-env /app/out .
ENTRYPOINT ["dotnet", "API.dll"]



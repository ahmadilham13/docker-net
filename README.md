## Getting Started

To Build ASP .NET 6 to Docker Container

## Prerequisites

Please make sure you have installed .NET 6+ and Docker on your computer

### Build & Run .NET

1. Build and Run the project for check your port on local
```sh
dotnet build && dotnet run
```

2. For Example my local server is 
```sh
Server started http://0.0.0.0:8080
```
or
```sh
http://localhost:8080
```

### Docker Compose

1. Create a file named docker-compose.yml in the root directory of your folder
```sh
version: "3.9"

services:
    app:
        container_name: app
        image: app
        build:
            context: .
            dockerfile: Dockerfile
            target: runtime
        restart: always
        ports:
            - "8080:8080"
```

### Dockerfile

1. Create a file named Dockerfile in the root directory of your folder
```sh
FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build

WORKDIR /app

COPY ./*.csproj /app/

RUN dotnet restore

FROM build as publish

WORKDIR /app

COPY . .

RUN dotnet publish ./App.csproj -c Release -o ./app/bin/Release/net

FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS runtime

WORKDIR /app

COPY --from=publish /app/app/bin/Release/net ./app/bin/Release/net

ENTRYPOINT ["dotnet", "./app/bin/Release/net/App.dll"]
```

Let's break it down line by line:

```sh
FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
```
This tells Docker to get the latest version of the sdk:6.0-alpine Docker image by mcr.microsoft.com/dotnet and name this image build.

```sh
WORKDIR /app
```
This tells Docker to switch to and run the app in the /app directory.

```sh
COPY ./*.csproj /app/
```

This tells Docker to copy our App.csproj file to the root of the container's working directory.

```sh
RUN dotnet restore
```

This runs the dotnet restore command which will download the NuGet packages for our project.

```sh
FROM build as publish
```

This starts a new stage called publish. This stage is used to publish the application.

```sh
WORKDIR /app
```

This tells Docker to switch to and run the app in the /app directory.

```sh
COPY . .
```

This tells Docker to copy the entire contents of the /app directory to the root of the container's working directory.

```sh
RUN dotnet publish ./App.csproj -c Release -o ./app/bin/Release/net
```

This runs the dotnet publish command which will build the application in release mode. We are using the -o flag to tell the command to output the application to the ./app/bin/Release/net directory.

```sh
FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS runtime
```

This tells Docker to get the latest version of the aspnet:6.0-alpine Docker image by mcr.microsoft.com/dotnet and name this image runtime.

```sh
WORKDIR /app
```

This tells Docker to switch to and run the app in the /app directory.

```sh
COPY --from=publish /app/app/bin/Release/net ./app/bin/Release/net
```

This tells Docker to copy the application from the publish stage to the runtime stage so that it can be run.

```sh
ENTRYPOINT ["dotnet", "./app/bin/Release/net/App.dll"]
```

Finally, this tells Docker to run the application using our compiled App.dll file.

### Running the app

Finally we are ready to deploy the app on a Docker container. To do so, use Docker Compose.

```sh
docker compose up -d --build
```

You can confirm the container is running in Docker, along with all of the other containers that are running by using the docker ps command.

```sh
docker ps
```


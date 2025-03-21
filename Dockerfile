FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /app

COPY ./blog/blog.csproj ./blog/
RUN dotnet restore ./blog/blog.csproj

COPY ./blog ./blog
WORKDIR /app/blog
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

COPY --from=build-env /app/blog/out ./
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "blog.dll"]

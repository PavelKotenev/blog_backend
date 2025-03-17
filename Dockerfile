# Этап 1: Сборка приложения
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /app

# Копируем csproj и восстанавливаем зависимости
COPY ./blog/blog.csproj ./blog/
RUN dotnet restore ./blog/blog.csproj

# Копируем весь проект и собираем
COPY ./blog ./blog
WORKDIR /app/blog
RUN dotnet publish -c Release -o out

# Этап 2: Минимальный образ для запуска приложения
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# Копируем собранные файлы из предыдущего этапа
COPY --from=build-env /app/blog/out ./

# Указываем точку входа для приложения
ENTRYPOINT ["dotnet", "Blog.dll"]

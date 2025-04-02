# Первая стадия: Сборка приложения
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /app

# Устанавливаем dotnet-ef
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

# Копируем .csproj файлы всех проектов (кроме тестов)
COPY ./Blog.Application/Blog.Application.csproj ./Blog.Application/
COPY ./Blog.Contracts/Blog.Contracts.csproj ./Blog.Contracts/
COPY ./Blog.Domain/Blog.Domain.csproj ./Blog.Domain/
COPY ./Blog.Infrastructure/Blog.Infrastructure.csproj ./Blog.Infrastructure/
COPY ./Blog.Presentation/Blog.Presentation.csproj ./Blog.Presentation/

# Восстанавливаем зависимости
RUN dotnet restore ./Blog.Presentation/Blog.Presentation.csproj

# Копируем исходный код всех проектов (кроме тестов)
COPY ./Blog.Application ./Blog.Application/
COPY ./Blog.Contracts ./Blog.Contracts/
COPY ./Blog.Domain ./Blog.Domain/
COPY ./Blog.Infrastructure ./Blog.Infrastructure/
COPY ./Blog.Presentation ./Blog.Presentation/

# Проверяем, что appsettings.Production.json скопировался
RUN ls -la /app/Blog.Presentation
RUN cat /app/Blog.Presentation/appsettings.Production.json

# Публикуем приложение
WORKDIR /app/Blog.Presentation
RUN dotnet publish -c Release -o out

# Вторая стадия: Итоговый образ
FROM mcr.microsoft.com/dotnet/sdk:9.0
WORKDIR /app

# Копируем скомпилированные файлы
COPY --from=build-env /app/Blog.Presentation/out ./
# Копируем исходный код для применения миграций
COPY --from=build-env /app/Blog.Application /app/Blog.Application/
COPY --from=build-env /app/Blog.Contracts /app/Blog.Contracts/
COPY --from=build-env /app/Blog.Domain /app/Blog.Domain/
COPY --from=build-env /app/Blog.Infrastructure /app/Blog.Infrastructure/
COPY --from=build-env /app/Blog.Presentation /app/Blog.Presentation/
# Копируем appsettings.Production.json
COPY --from=build-env /app/Blog.Presentation/appsettings.Production.json ./

# Устанавливаем dotnet-ef в итоговый образ
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

# Устанавливаем переменные окружения
ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT=Production

# Запускаем приложение
ENTRYPOINT ["dotnet", "blog.dll"]
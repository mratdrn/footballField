# Halı Saha Rezervasyon API

ASP.NET Core 9 ile geliştirilmiş RESTful API projesi.

## Özellikler
- JWT ile kimlik doğrulama ve yetkilendirme
- Rol tabanlı erişim kontrolü (Admin / Üye)
- Saha yönetimi (CRUD)
- Çakışma kontrolü ile rezervasyon sistemi
- Entity Framework Core + PostgreSQL
- Swagger UI

## Teknolojiler
- ASP.NET Core 9 Web API
- Entity Framework Core 9
- PostgreSQL
- JWT Bearer Authentication
- BCrypt şifreleme
- Swagger / OpenAPI

## Kurulum
1. Repoyu klonla
2. appsettings.json içindeki connection string'i güncelle
3. dotnet ef database update
4. dotnet run
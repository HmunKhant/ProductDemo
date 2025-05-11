# 🛍️ Product Demo Web Application

This is a simple ASP.NET Core MVC application for managing products. It uses a layered architecture with **Repositories**, **Services**, **Dapper ORM**, and **JWT-based authentication**.

---

## 🔐 Login Credentials

- **Username**: `Admin`  
- **Password**: `1234`

> The password is securely hashed using PBKDF2 with SHA256.

---

## 🚀 Features

- ✅ JWT-based user authentication with cookies
- 📦 CRUD operations for products
- 🔍 Product filtering by name, min price, and max price
- 📄 Pagination for product list
- 💻 Responsive UI with Bootstrap 5 (Lux theme)
- 🛡️ Protected routes using authentication middleware
- ❗ Global error handling via middleware
- 🗂️ Clean MVC architecture using Repositories and Services

---

## 🛠️ Technologies Used

- .NET 8 (ASP.NET Core MVC)
- Dapper ORM
- SQL Server
- JWT Authentication
- Bootstrap 5 (Bootswatch Lux theme)

---

## 🗄️ Database Setup

### 1. Create Database and Tables

```sql
CREATE DATABASE ProductDemoDb;
USE [ProductDemoDb];

CREATE TABLE [admin_user] (
    id INT IDENTITY(1,1),
    username VARCHAR(50) NOT NULL,
    password VARCHAR(MAX) NOT NULL,
    created_at DATETIME NOT NULL DEFAULT GETDATE(),
    updated_at DATETIME,
    created_by INT NOT NULL,
    updated_by INT
);

CREATE TABLE [product] (
    id INT IDENTITY(1,1),
    name NVARCHAR(255) NOT NULL,
    price DECIMAL(18,2) NOT NULL,
    description NVARCHAR(1000),
    created_at DATETIME NOT NULL DEFAULT GETDATE(),
    updated_at DATETIME,
    created_by INT NOT NULL,
    updated_by INT
);
```

### 2. Seed Admin User

```sql
INSERT INTO [dbo].[admin_user]
    ([username], [password], [created_at], [updated_at], [created_by], [updated_by])
VALUES
    ('Admin', 
     '20PInZAsVR0ip6S3TdDn5BD0H1YtMXobeHJuSDZwqP4eUf0cgeBNyn/YSNWuXS5m', 
     GETDATE(), 
     NULL, 
     1, 
     NULL);
```

---

## ⚙️ Getting Started

1. **Clone the repository**

```bash
git clone https://github.com/HmunKhant/ProductDemo.git
cd ProductDemo
```

2. **Configure `appsettings.json`**

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=ProductDemoDb;Trusted_Connection=True;"
},
"JwtBearer": {
  "AccessKey": "p7H3v9uTzYb4B9Q2m4dF8nJ5v3xMZqLpXJ7+4dG1PQo=",
  "RefreshKey": "XbQ5dV97r2PzJv4YtM8K9LzG5XjN2Rb1fYQ3+T7H4P8=",
  "Issuer": "ProductDemo",
  "Audience": "DemoAudience",
  "AccessTokenExpiration": 3600,
  "RefreshTokenExpiration": 36000
},
```

3. **Run the app**

- Using CLI:

```bash
dotnet restore
dotnet run
```

- Or press `F5` in Visual Studio

---
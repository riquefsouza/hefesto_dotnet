http://localhost:5000/swagger/index.html

ou

https://localhost:5001/swagger/index.html

This package helps generate controllers and views.
Install-Package Microsoft.VisualStudio.Web.CodeGeneration.Design -Version 3.1.4

This package helps create database context and model classes from the database.
Install-Package Microsoft.EntityFrameworkCore.Tools -Version 3.1.8

Database provider allows Entity Framework Core to work with SQL Server.
Install-Package Microsoft.EntityFrameworkCore.SqlServer -Version 3.1.8

It provides support for creating and validating a JWT token.
Install-Package System.IdentityModel.Tokens.Jwt -Version 5.6.0

This is the middleware that enables an ASP.NET Core application to receive a bearer token in the request pipeline.
Install-Package Microsoft.AspNetCore.Authentication.JwtBearer -Version 3.1.8

dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package System.IdentityModel.Tokens.Jwt
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer

dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL.Design
dotnet add package Pomelo.EntityFrameworkCore.MySql
dotnet add package Microsoft.EntityFrameworkCore.SqlServer

dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.Tools.DotNet

dotnet add package BCrypt.Net-Next

dotnet add package Microsoft.AspNetCore.Mvc.Versioning

dotnet add package System.Data.Common
dotnet add package System.Linq.Expressions

dotnet add package Microsoft.EntityFrameworkCore.Relational
dotnet add package Dapper

dotnet restore

dotnet ef dbcontext scaffold "Host=localhost;Database=dbhefesto;Username=postgres;Password=abcd1234" Npgsql.EntityFrameworkCore.PostgreSQL -o Models

dotnet add package Microsoft.EntityFrameworkCore.SqlServer

dotnet tool install -g dotnet-aspnet-codegenerator
dotnet aspnet-codegenerator controller -name AdmParameterCategoryController -async -api -m AdmParameterCategory -dc dbhefestoContext -outDir Controllers

dotnet aspnet-codegenerator controller -name AdmParameterController -async -api -m AdmParameter -dc dbhefestoContext -outDir admin\Controllers

dotnet aspnet-codegenerator controller -name AdmMenuController -async -api -m AdmMenu -dc dbhefestoContext -outDir admin\Controllers

dotnet aspnet-codegenerator controller -name AdmPageController -async -api -m AdmPage -dc dbhefestoContext -outDir admin\Controllers

dotnet aspnet-codegenerator controller -name AdmUserController -async -api -m AdmUser -dc dbhefestoContext -outDir admin\Controllers

dotnet aspnet-codegenerator controller -name AdmProfileController -async -api -m AdmProfile -dc dbhefestoContext -outDir admin\Controllers

# Hefesto dotNet API

```
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
```

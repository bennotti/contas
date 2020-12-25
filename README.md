# Contas

### Login/Autenticação
```sh
$ Usuário: admin
$ Senha: admin
```

## Contas-Backend

### Tecnologias:
  - C# Core 3.1
  - EntityFramework Core
  - MySql
  - Swagger
  - JWT
  - IIS Express

### Add-migration
```sh
$ dotnet ef migrations add {NOME_DA_MIGRATION} --project ./Contas.Backend/Contas.Core --startup-project ./Contas.Backend/Contas.Api
```

### Update-database
```sh
$ dotnet ef database update --project ./Contas.Backend/Contas.Core --startup-project ./Contas.Backend/Contas.Api
```

## Contas-WebApp

### Tecnologias:
  - VueJs
  - Bootstrap
  - JWT
  - Vue-Cli

### Instalação
```sh
$ yarn install
```

### Desenvolvimento
```sh
$ yarn run serve
```

### Build
```sh
$ yarn run build
```

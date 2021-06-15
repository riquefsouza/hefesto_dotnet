# Hefesto Dotnet GraphQL

https://localhost:5001/graphql/

https://localhost:5001/graphql-voyager


```
dotnet new web -n hefesto_dotnet_graphql

code -r hefesto_dotnet_graphql

dotnet add package HotChocolate.AspNetCore

dotnet add package HotChocolate.Data.Entityframework

dotnet add package Microsoft.EntityframeworkCore.Design

dotnet add package GraphQL.Server.Ui.Voyager

dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL.Design

```

```
query{
  admParameterCategory{
    id
    description
    admParameters{
      id
      description
    }
  }
}

query{
  admParameter{
    id
    description
    admParameterCategory{
      id
      description
    }
  }
}

query{
  admParameter(where: {id: {eq: 1}}){
    id
    description
    admParameterCategory{
      id
      description
    }
  }
}

query{
  admParameter(order: {description: DESC}){
    id
    description
    admParameterCategory{
      id
      description
    }
  }
}
```

```
mutation{
  admParameterCategoryInsert(input: {
    description: "ALFA TESTE",
    order: 23
  }){
    admParameterCategory{
      id
      description
      order
      admParameters{
        id
        description
      }
    }
  }
}

mutation{
  admParameterCategoryUpdate(id: 43, input: {
    description: "BETA",
    order: 32
  }){
    admParameterCategory{
      id
      description
      order
      admParameters{
        id
        description
      }
    }
  }
}


mutation{
  admParameterCategoryDelete(id: 40)
}


```
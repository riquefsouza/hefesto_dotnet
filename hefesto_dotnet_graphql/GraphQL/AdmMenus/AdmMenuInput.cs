using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hefesto_dotnet_graphql.GraphQL.AdmMenus
{
    public record AdmMenuInput(string Description, int Order, long IdPage, long IdMenuParent);
}

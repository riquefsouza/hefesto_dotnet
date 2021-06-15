using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using hefesto.admin.Models;

namespace hefesto_dotnet_graphql.GraphQL
{
    public class Subscription
    {
        [Subscribe]
        [Topic]
        public AdmParameterCategory OnAdmParameterCategoryInserted(
            [EventMessage] AdmParameterCategory admParameterCategory)
        {
            return admParameterCategory;
        }
    }
}

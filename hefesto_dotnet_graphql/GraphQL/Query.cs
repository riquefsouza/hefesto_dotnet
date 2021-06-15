using System.Linq;
using HotChocolate;
using hefesto.admin.Models;

namespace hefesto_dotnet_graphql.GraphQL
{
    public class Query
    {
        public IQueryable<AdmParameterCategory> GetAdmParameterCategory([Service] dbhefestoContext context)
        {
            return context.AdmParameterCategories;
        }
    }
}
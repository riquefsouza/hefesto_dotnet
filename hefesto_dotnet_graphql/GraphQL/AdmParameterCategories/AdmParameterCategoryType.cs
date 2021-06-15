using hefesto.admin.Models;
using HotChocolate;
using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hefesto_dotnet_graphql.GraphQL.AdmParameterCategories
{
    public class AdmParameterCategoryType : ObjectType<AdmParameterCategory>
    {
        protected override void Configure(IObjectTypeDescriptor<AdmParameterCategory> descriptor)
        {
            descriptor.Description("Represents the parameter category");

            descriptor.Field(p => p.AdmParameters)
                .ResolveWith<Resolvers>(p => p.GetAdmParameters(default!, default!))
                .UseDbContext<dbhefestoContext>()
                .Description("This is the list of availble parameters for this category");
        }

        private class Resolvers
        {
            public IQueryable<AdmParameter> GetAdmParameters(AdmParameterCategory admParameterCategory, 
                [ScopedService] dbhefestoContext context)
            {
                return context.AdmParameters.Where(p => p.IdParameterCategory == admParameterCategory.Id);
            }
        }
    }
}

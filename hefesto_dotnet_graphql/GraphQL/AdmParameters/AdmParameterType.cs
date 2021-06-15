using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hefesto.admin.Models;
using HotChocolate;
using HotChocolate.Types;

namespace hefesto_dotnet_graphql.GraphQL.AdmParameters
{
    public class AdmParameterType : ObjectType<AdmParameter>
    {
        protected override void Configure(IObjectTypeDescriptor<AdmParameter> descriptor)
        {
            descriptor.Description("Represents the parameter");

            descriptor.Field(p => p.AdmParameterCategory)
            .ResolveWith<Resolvers>(p => p.GetAdmParameterCategoryies(default!, default!))
            .UseDbContext<dbhefestoContext>()
            .Description("This is the category to which the parameter belongs");
        }

        private class Resolvers
        {
            public AdmParameterCategory GetAdmParameterCategoryies(AdmParameter admParameter, 
                [ScopedService] dbhefestoContext context)
            {
                return context.AdmParameterCategories.FirstOrDefault(p => p.Id == admParameter.Id);
            }
        }
    }
}

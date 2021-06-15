using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hefesto.admin.Models;
using hefesto.admin.Services;
using HotChocolate;
using HotChocolate.Types;

namespace hefesto_dotnet_graphql.GraphQL.AdmUsers
{
    public class AdmUserType : ObjectType<AdmUser>
    {
        protected override void Configure(IObjectTypeDescriptor<AdmUser> descriptor)
        {
            descriptor.Description("Represents the user");

            descriptor.Field(p => p.AdmUserProfiles)
            .ResolveWith<Resolvers>(p => p.GetAdmUserProfiles(default!, default!))
            .UseDbContext<dbhefestoContext>()
            .Description("This is the list of availble profiles for this user");
        }

        private class Resolvers
        {
            public IQueryable<AdmUserProfile> GetAdmUserProfiles(AdmUser admUser, [ScopedService] dbhefestoContext context)
            {
                return context.AdmUserProfiles.Where(p => p.IdUser == admUser.Id);
            }
        }
    }
}
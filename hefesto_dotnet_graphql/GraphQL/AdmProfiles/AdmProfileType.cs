using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hefesto.admin.Models;
using hefesto.admin.Services;
using HotChocolate;
using HotChocolate.Types;

namespace hefesto_dotnet_graphql.GraphQL.AdmProfiles
{
    public class AdmProfileType : ObjectType<AdmProfile>
    {
        protected override void Configure(IObjectTypeDescriptor<AdmProfile> descriptor)
        {
            descriptor.Description("Represents the profile");

            descriptor.Field(p => p.AdmPageProfiles)
            .ResolveWith<Resolvers>(p => p.GetAdmPageProfiles(default!, default!))
            .UseDbContext<dbhefestoContext>()
            .Description("This is the list of availble pages for this profile");

            descriptor.Field(p => p.AdmUserProfiles)
            .ResolveWith<Resolvers>(p => p.GetAdmUserProfiles(default!, default!))
            .UseDbContext<dbhefestoContext>()
            .Description("This is the list of availble users for this profile");
        }

        private class Resolvers
        {
            public IQueryable<AdmPageProfile> GetAdmPageProfiles(AdmProfile admProfile, [ScopedService] dbhefestoContext context)
            {
                return context.AdmPageProfiles.Where(p => p.IdProfile == admProfile.Id);
            }

            public IQueryable<AdmUserProfile> GetAdmUserProfiles(AdmProfile admProfile, [ScopedService] dbhefestoContext context)
            {
                return context.AdmUserProfiles.Where(p => p.IdProfile == admProfile.Id);
            }
        }
    }
}

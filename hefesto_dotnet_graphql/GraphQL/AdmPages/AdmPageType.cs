using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hefesto.admin.Models;
using hefesto.admin.Services;
using HotChocolate;
using HotChocolate.Types;

namespace hefesto_dotnet_graphql.GraphQL.AdmPages
{
    public class AdmPageType : ObjectType<AdmPage>
    {
        protected override void Configure(IObjectTypeDescriptor<AdmPage> descriptor)
        {
            descriptor.Description("Represents the page");

            descriptor.Field(p => p.AdmMenus)
            .ResolveWith<Resolvers>(p => p.GetAdmMenus(default!, default!))
            .UseDbContext<dbhefestoContext>()
            .Description("This is the list of availble menus for this page");

            descriptor.Field(p => p.AdmPageProfiles)
            .ResolveWith<Resolvers>(p => p.GetAdmPageProfiles(default!, default!))
            .UseDbContext<dbhefestoContext>()
            .Description("This is the list of availble profiles for this page");
        }

        private class Resolvers
        {
            public IQueryable<AdmMenu> GetAdmMenus(AdmPage admPage, [ScopedService] dbhefestoContext context)
            {
                return context.AdmMenus.Where(p => p.IdPage == admPage.Id);
            }

            public IQueryable<AdmPageProfile> GetAdmPageProfiles(AdmPage admPage, [ScopedService] dbhefestoContext context)
            {
                return context.AdmPageProfiles.Where(p => p.IdPage == admPage.Id);
            }
        }
    }
}

using System.Linq;
using hefesto.admin.Models;
using HotChocolate;
using HotChocolate.Types;

namespace hefesto_dotnet_graphql.GraphQL.AdmMenus
{
    public class AdmMenuType : ObjectType<AdmMenu>
    {
        protected override void Configure(IObjectTypeDescriptor<AdmMenu> descriptor)
        {
            descriptor.Description("Represents the menu");

            descriptor.Field(p => p.AdmPage)
            .ResolveWith<Resolvers>(p => p.GetAdmPage(default!, default!))
            .UseDbContext<dbhefestoContext>()
            .Description("This is the page to which the menu belongs");

            descriptor.Field(p => p.AdmMenuParent)
            .ResolveWith<Resolvers>(p => p.GetAdmMenuParent(default!, default!))
            .UseDbContext<dbhefestoContext>()
            .Description("This is the menu parent to which the menu belongs");

            descriptor.Field(p => p.AdmSubMenus)
            .ResolveWith<Resolvers>(p => p.GetAdmSubMenus(default!, default!))
            .UseDbContext<dbhefestoContext>()
            .Description("This is the list of availble submenus for this menu");
        }

        private class Resolvers
        {
            public AdmPage GetAdmPage(AdmMenu admMenu, [ScopedService] dbhefestoContext context)
            {
                return context.AdmPages.FirstOrDefault(p => p.Id == admMenu.Id);
            }

            public AdmMenu GetAdmMenuParent(AdmMenu admMenu, [ScopedService] dbhefestoContext context)
            {
                return context.AdmMenus.FirstOrDefault(p => p.Id == admMenu.IdMenuParent);
            }

            public IQueryable<AdmMenu> GetAdmSubMenus(AdmMenu admMenu, [ScopedService] dbhefestoContext context)
            {
                return context.AdmMenus.Where(p => p.IdMenuParent == admMenu.Id);
            }
        }
    }
}

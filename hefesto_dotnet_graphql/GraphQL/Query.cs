using System.Linq;
using HotChocolate;
using HotChocolate.Data;
using hefesto.admin.Models;

namespace hefesto_dotnet_graphql.GraphQL
{
    public class Query
    {
        /*
        public IQueryable<AdmParameterCategory> GetAdmParameterCategory([Service] dbhefestoContext context)
        {
            return context.AdmParameterCategories;
        }
        */

        [UseDbContext(typeof(dbhefestoContext))]
        //[UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<AdmParameterCategory> GetAdmParameterCategory([ScopedService] dbhefestoContext context)
        {
            return context.AdmParameterCategories;
        }

        [UseDbContext(typeof(dbhefestoContext))]
        //[UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<AdmParameter> GetAdmParameter([ScopedService] dbhefestoContext context)
        {
            return context.AdmParameters;
        }

        [UseDbContext(typeof(dbhefestoContext))]
        //[UseProjection]
        public IQueryable<AdmMenu> GetAdmMenu([ScopedService] dbhefestoContext context)
        {
            return context.AdmMenus;
        }

        [UseDbContext(typeof(dbhefestoContext))]
        //[UseProjection]
        public IQueryable<AdmPage> GetAdmPage([ScopedService] dbhefestoContext context)
        {
            return context.AdmPages;
        }

        [UseDbContext(typeof(dbhefestoContext))]
        //[UseProjection]
        public IQueryable<AdmProfile> GetAdmProfile([ScopedService] dbhefestoContext context)
        {
            return context.AdmProfiles;
        }

        [UseDbContext(typeof(dbhefestoContext))]
        //[UseProjection]
        public IQueryable<AdmUser> GetAdmUser([ScopedService] dbhefestoContext context)
        {
            return context.AdmUsers;
        }
    }
}
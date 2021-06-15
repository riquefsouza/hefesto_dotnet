using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hefesto_dotnet_graphql.GraphQL.AdmProfiles
{
    public record AdmProfileInput(bool? Administrator, string Description, bool? General,
        List<long> IdAdmPages, List<long> IdAdmUsers);
}

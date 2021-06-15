using hefesto.admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hefesto_dotnet_graphql.GraphQL.AdmUsers
{
    public record AdmUserInput(bool? Active, string Email, string Login, string Name, string Password,
        List<long> admIdProfiles);
}

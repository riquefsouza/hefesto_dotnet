using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hefesto_dotnet_graphql.GraphQL.AdmParameters
{
    public record AdmParameterInput(string Code, string Description, long IdParameterCategory, string Value);
}

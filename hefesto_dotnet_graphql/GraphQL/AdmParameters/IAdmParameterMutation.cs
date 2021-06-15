using System.Threading.Tasks;

namespace hefesto_dotnet_graphql.GraphQL.AdmParameters
{
    public interface IAdmParameterMutation
    {
        Task<AdmParameterPayload> AdmParameterInsertAsync(AdmParameterInput input);

        Task<AdmParameterPayload> AdmParameterUpdateAsync(long id,
            AdmParameterInput input);
        Task<bool> AdmParameterDeleteAsync(long id);
    }
}
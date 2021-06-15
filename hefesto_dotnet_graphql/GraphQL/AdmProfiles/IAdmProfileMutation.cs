using System.Threading.Tasks;

namespace hefesto_dotnet_graphql.GraphQL.AdmProfiles
{
    public interface IAdmProfileMutation
    {
        Task<AdmProfilePayload> AdmProfileInsertAsync(AdmProfileInput input);

        Task<AdmProfilePayload> AdmProfileUpdateAsync(long id,
            AdmProfileInput input);
        Task<bool> AdmProfileDeleteAsync(long id);
    }
}

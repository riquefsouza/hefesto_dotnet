using System.Threading.Tasks;

namespace hefesto_dotnet_graphql.GraphQL.AdmPages
{
    public interface IAdmPageMutation
    {
        Task<AdmPagePayload> AdmPageInsertAsync(AdmPageInput input);

        Task<AdmPagePayload> AdmPageUpdateAsync(long id,
            AdmPageInput input);
        Task<bool> AdmPageDeleteAsync(long id);
    }
}

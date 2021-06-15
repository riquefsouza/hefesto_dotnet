using System.Threading.Tasks;

namespace hefesto_dotnet_graphql.GraphQL.AdmUsers
{
    public interface IAdmUserMutation
    {
        Task<AdmUserPayload> AdmUserInsertAsync(AdmUserInput input);

        Task<AdmUserPayload> AdmUserUpdateAsync(long id,
            AdmUserInput input);
        Task<bool> AdmUserDeleteAsync(long id);
    }
}

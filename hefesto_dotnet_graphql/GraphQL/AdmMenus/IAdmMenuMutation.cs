using System.Threading.Tasks;

namespace hefesto_dotnet_graphql.GraphQL.AdmMenus
{
    public interface IAdmMenuMutation
    {
        Task<AdmMenuPayload> AdmMenuInsertAsync(AdmMenuInput input);

        Task<AdmMenuPayload> AdmMenuUpdateAsync(long id,
            AdmMenuInput input);
        Task<bool> AdmMenuDeleteAsync(long id);
    }
}

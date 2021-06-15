using System.Threading.Tasks;
using HotChocolate;
using hefesto.admin.Models;
using System.Threading;
using HotChocolate.Subscriptions;

namespace hefesto_dotnet_graphql.GraphQL.AdmParameterCategories
{
    public interface IAdmParameterCategoryMutation
    {
        Task<AdmParameterCategoryPayload> AdmParameterCategoryInsertAsync(
            AdmParameterCategoryInput input, [ScopedService] dbhefestoContext context,
            [Service] ITopicEventSender eventSender, CancellationToken cancellationToken);

        Task<AdmParameterCategoryPayload> AdmParameterCategoryUpdateAsync(long id,
            AdmParameterCategoryInput input);
        Task<bool> AdmParameterCategoryDeleteAsync(long id);
    }
}
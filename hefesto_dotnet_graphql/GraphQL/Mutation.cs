using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Data;
using hefesto.admin.Models;
using System.Threading;
using HotChocolate.Subscriptions;
using hefesto_dotnet_graphql.GraphQL.AdmParameterCategories;
using hefesto_dotnet_graphql.GraphQL.AdmParameters;

namespace hefesto_dotnet_graphql.GraphQL
{
    public class Mutation
    {
        private readonly IAdmParameterCategoryMutation admParameterCategoryMutation;
        private readonly IAdmParameterMutation admParameterMutation;

        public Mutation(IAdmParameterCategoryMutation admParameterCategoryMutation,
            IAdmParameterMutation admParameterMutation)
        {
            this.admParameterCategoryMutation = admParameterCategoryMutation;
            this.admParameterMutation = admParameterMutation;
        }

        [UseDbContext(typeof(dbhefestoContext))]
        public async Task<AdmParameterCategoryPayload> AdmParameterCategoryInsert(
            AdmParameterCategoryInput input, [ScopedService] dbhefestoContext context,
            [Service] ITopicEventSender eventSender, CancellationToken cancellationToken)
        {
            return await admParameterCategoryMutation.AdmParameterCategoryInsertAsync(
                input, context, eventSender, cancellationToken);
        }

        public async Task<AdmParameterCategoryPayload> AdmParameterCategoryUpdate(long id,
            AdmParameterCategoryInput input)
        {
            return await admParameterCategoryMutation.AdmParameterCategoryUpdateAsync(id, input);
        }

        public async Task<bool> AdmParameterCategoryDeleteAsync(long id)
        {
            return await admParameterCategoryMutation.AdmParameterCategoryDeleteAsync(id);
        }

        public async Task<AdmParameterPayload> AdmParameterInsert(AdmParameterInput input)
        {
            return await admParameterMutation.AdmParameterInsertAsync(input);
        }

        public async Task<AdmParameterPayload> AdmParameterUpdate(long id, AdmParameterInput input)
        {
            return await admParameterMutation.AdmParameterUpdateAsync(id, input);
        }

        public async Task<bool> AdmParameterDelete(long id)
        {
            return await admParameterMutation.AdmParameterDeleteAsync(id);
        }
    }
}

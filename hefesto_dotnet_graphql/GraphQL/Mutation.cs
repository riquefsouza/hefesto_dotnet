using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Data;
using hefesto.admin.Models;
using System.Threading;
using HotChocolate.Subscriptions;
using hefesto_dotnet_graphql.GraphQL.AdmParameterCategories;
using hefesto_dotnet_graphql.GraphQL.AdmParameters;
using hefesto_dotnet_graphql.GraphQL.AdmMenus;
using hefesto_dotnet_graphql.GraphQL.AdmPages;
using hefesto_dotnet_graphql.GraphQL.AdmProfiles;
using hefesto_dotnet_graphql.GraphQL.AdmUsers;

namespace hefesto_dotnet_graphql.GraphQL
{
    public class Mutation
    {
        private readonly IAdmParameterCategoryMutation admParameterCategoryMutation;
        private readonly IAdmParameterMutation admParameterMutation;
        private readonly IAdmMenuMutation admMenuMutation;
        private readonly IAdmPageMutation admPageMutation;
        private readonly IAdmProfileMutation admProfileMutation;
        private readonly IAdmUserMutation admUserMutation;

        public Mutation(IAdmParameterCategoryMutation admParameterCategoryMutation,
            IAdmParameterMutation admParameterMutation, IAdmMenuMutation admMenuMutation,
            IAdmPageMutation admPageMutation, IAdmProfileMutation admProfileMutation,
            IAdmUserMutation admUserMutation)
        {
            this.admParameterCategoryMutation = admParameterCategoryMutation;
            this.admParameterMutation = admParameterMutation;
            this.admMenuMutation = admMenuMutation;
            this.admPageMutation = admPageMutation;
            this.admProfileMutation = admProfileMutation;
            this.admUserMutation = admUserMutation;
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

        public async Task<AdmMenuPayload> AdmMenuInsert(AdmMenuInput input)
        {
            return await admMenuMutation.AdmMenuInsertAsync(input);
        }

        public async Task<AdmMenuPayload> AdmMenuUpdate(long id, AdmMenuInput input)
        {
            return await admMenuMutation.AdmMenuUpdateAsync(id, input);
        }

        public async Task<bool> AdmMenuDelete(long id)
        {
            return await admMenuMutation.AdmMenuDeleteAsync(id);
        }

        public async Task<AdmPagePayload> AdmPageInsert(AdmPageInput input)
        {
            return await admPageMutation.AdmPageInsertAsync(input);
        }

        public async Task<AdmPagePayload> AdmPageUpdate(long id, AdmPageInput input)
        {
            return await admPageMutation.AdmPageUpdateAsync(id, input);
        }

        public async Task<bool> AdmPageDelete(long id)
        {
            return await admPageMutation.AdmPageDeleteAsync(id);
        }

        public async Task<AdmProfilePayload> AdmProfileInsert(AdmProfileInput input)
        {
            return await admProfileMutation.AdmProfileInsertAsync(input);
        }

        public async Task<AdmProfilePayload> AdmProfileUpdate(long id, AdmProfileInput input)
        {
            return await admProfileMutation.AdmProfileUpdateAsync(id, input);
        }

        public async Task<bool> AdmProfileDelete(long id)
        {
            return await admProfileMutation.AdmProfileDeleteAsync(id);
        }

        public async Task<AdmUserPayload> AdmUserInsert(AdmUserInput input)
        {
            return await admUserMutation.AdmUserInsertAsync(input);
        }

        public async Task<AdmUserPayload> AdmUserUpdate(long id, AdmUserInput input)
        {
            return await admUserMutation.AdmUserUpdateAsync(id, input);
        }

        public async Task<bool> AdmUserDelete(long id)
        {
            return await admUserMutation.AdmUserDeleteAsync(id);
        }

    }
}

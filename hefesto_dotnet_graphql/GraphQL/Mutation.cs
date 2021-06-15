using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Data;
using hefesto.admin.Models;
using hefesto_dotnet_graphql.GraphQL.AdmParameterCategories;
using System.Threading;
using HotChocolate.Subscriptions;

namespace hefesto_dotnet_graphql.GraphQL
{
    public class Mutation
    {
        [UseDbContext(typeof(dbhefestoContext))]
        public async Task<AdmParameterCategoryPayload> AdmParameterCategoryInsertAsync(
            AdmParameterCategoryInput input,
            [ScopedService] dbhefestoContext context,
            [Service] ITopicEventSender eventSender,
            CancellationToken cancellationToken)
        {
            var obj = new AdmParameterCategory
            {
                Description = input.Description,
                Order = input.Order
            };

            context.AdmParameterCategories.Add(obj);
            await context.SaveChangesAsync(cancellationToken);

            await eventSender.SendAsync(nameof(Subscription.OnAdmParameterCategoryInserted),
                obj, cancellationToken);

            return new AdmParameterCategoryPayload(obj);
        }
    }
}

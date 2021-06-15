using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Data;
using hefesto.admin.Models;
using System.Threading;
using HotChocolate.Subscriptions;
using hefesto.admin.Services;

namespace hefesto_dotnet_graphql.GraphQL.AdmParameterCategories
{
    public class AdmParameterCategoryMutation : IAdmParameterCategoryMutation
    {
        private readonly IAdmParameterCategoryService service;

        public AdmParameterCategoryMutation(IAdmParameterCategoryService service)
        {
            this.service = service;
        }

        private AdmParameterCategory SetObj(long? id, AdmParameterCategoryInput input)
        {
            var obj = new AdmParameterCategory
            {
                Description = input.Description,
                Order = input.Order
            };

            if (id != null)
            {
                obj.Id = (long)id;
            }

            return obj;
        }

        [UseDbContext(typeof(dbhefestoContext))]
        public async Task<AdmParameterCategoryPayload> AdmParameterCategoryInsertAsync(
            AdmParameterCategoryInput input,
            [ScopedService] dbhefestoContext context,
            [Service] ITopicEventSender eventSender,
            CancellationToken cancellationToken)
        {
            var obj = SetObj(null, input);

            obj.Id = this.service.GetNextSequenceValue();

            context.AdmParameterCategories.Add(obj);
            await context.SaveChangesAsync(cancellationToken);

            await eventSender.SendAsync(nameof(Subscription.OnAdmParameterCategoryInserted),
                obj, cancellationToken);

            return new AdmParameterCategoryPayload(obj);
        }

        //[UseDbContext(typeof(dbhefestoContext))]
        public async Task<AdmParameterCategoryPayload> AdmParameterCategoryUpdateAsync(long id,
            AdmParameterCategoryInput input)
            //[ScopedService] dbhefestoContext context)
        {
            var obj = SetObj(id, input);

            await this.service.Update(obj.Id, obj);

            return new AdmParameterCategoryPayload(obj);
        }

        //[UseDbContext(typeof(dbhefestoContext))]
        public async Task<bool> AdmParameterCategoryDeleteAsync(long id)
            //[ScopedService] dbhefestoContext context)
        {
            var ret = await this.service.Delete(id);

            return ret;
        }

    }
}

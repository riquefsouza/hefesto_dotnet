using System.Threading.Tasks;
using hefesto.admin.Models;
using hefesto.admin.Services;

namespace hefesto_dotnet_graphql.GraphQL.AdmPages
{
    public class AdmPageMutation : IAdmPageMutation
    {
        private readonly IAdmPageService service;

        public AdmPageMutation(IAdmPageService service)
        {
            this.service = service;
        }

        private AdmPage SetObj(long? id, AdmPageInput input)
        {
            var obj = new AdmPage
            {
                Description = input.Description,
                Url = input.Url
            };

            if (id != null)
            {
                obj.Id = (long)id;
            }

            return obj;
        }

        public async Task<AdmPagePayload> AdmPageInsertAsync(AdmPageInput input)
        {
            var obj = SetObj(null, input);

            await this.service.Insert(obj);

            return new AdmPagePayload(obj);
        }

        public async Task<AdmPagePayload> AdmPageUpdateAsync(long id,
            AdmPageInput input)
        {
            var obj = SetObj(id, input);

            await this.service.Update(obj.Id, obj);

            return new AdmPagePayload(obj);
        }

        public async Task<bool> AdmPageDeleteAsync(long id)
        {
            var ret = await this.service.Delete(id);

            return ret;
        }
    }
}

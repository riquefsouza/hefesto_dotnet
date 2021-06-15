using System.Threading.Tasks;
using hefesto.admin.Models;
using hefesto.admin.Services;

namespace hefesto_dotnet_graphql.GraphQL.AdmMenus
{
    public class AdmMenuMutation : IAdmMenuMutation
    {
        private readonly IAdmMenuService service;

        public AdmMenuMutation(IAdmMenuService service)
        {
            this.service = service;
        }

        private AdmMenu SetObj(long? id, AdmMenuInput input)
        {
            var obj = new AdmMenu
            {
                Description = input.Description,
                Order = input.Order,
                IdPage = input.IdPage,
                IdMenuParent = input.IdMenuParent
            };

            if (id != null)
            {
                obj.Id = (long)id;
            }

            return obj;
        }

        public async Task<AdmMenuPayload> AdmMenuInsertAsync(AdmMenuInput input)
        {
            var obj = SetObj(null, input);

            await this.service.Insert(obj);

            return new AdmMenuPayload(obj);
        }

        public async Task<AdmMenuPayload> AdmMenuUpdateAsync(long id,
            AdmMenuInput input)
        {
            var obj = SetObj(id, input);

            await this.service.Update(obj.Id, obj);

            return new AdmMenuPayload(obj);
        }

        public async Task<bool> AdmMenuDeleteAsync(long id)
        {
            var ret = await this.service.Delete(id);

            return ret;
        }
    }
}

using System.Threading.Tasks;
using hefesto.admin.Models;
using hefesto.admin.Services;

namespace hefesto_dotnet_graphql.GraphQL.AdmParameters
{
    public class AdmParameterMutation : IAdmParameterMutation
    {
        private readonly IAdmParameterService service;

        public AdmParameterMutation(IAdmParameterService service)
        {
            this.service = service;
        }

        private AdmParameter SetObj(long? id, AdmParameterInput input)
        {
            var obj = new AdmParameter
            {
                Description = input.Description,
                Code = input.Code,
                IdParameterCategory = input.IdParameterCategory,
                Value = input.Value
            };

            if (id != null)
            {
                obj.Id = (long)id;
            }

            return obj;
        }

        public async Task<AdmParameterPayload> AdmParameterInsertAsync(AdmParameterInput input)
        {
            var obj = SetObj(null, input);

            await this.service.Insert(obj);

            return new AdmParameterPayload(obj);
        }

        public async Task<AdmParameterPayload> AdmParameterUpdateAsync(long id,
            AdmParameterInput input)
        {
            var obj = SetObj(id, input);

            await this.service.Update(obj.Id, obj);

            return new AdmParameterPayload(obj);
        }

        public async Task<bool> AdmParameterDeleteAsync(long id)
        {
            var ret = await this.service.Delete(id);

            return ret;
        }
    }
}

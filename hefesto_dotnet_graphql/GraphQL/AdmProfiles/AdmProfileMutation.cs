using System.Linq;
using System.Threading.Tasks;
using hefesto.admin.Models;
using hefesto.admin.Services;

namespace hefesto_dotnet_graphql.GraphQL.AdmProfiles
{
    public class AdmProfileMutation : IAdmProfileMutation
    {
        private readonly IAdmProfileService service;

        public AdmProfileMutation(IAdmProfileService service)
        {
            this.service = service;
        }

        private AdmProfile SetObj(long? id, AdmProfileInput input)
        {
            var pageProfiles = input.IdAdmPages
                .Select(pageId => new AdmPageProfile(pageId, (long)id))
                .ToList<AdmPageProfile>();

            var userProfiles = input.IdAdmUsers
                .Select(userId => new AdmUserProfile(userId, (long)id))
                .ToList<AdmUserProfile>();

            var obj = new AdmProfile
            {
                Administrator = input.Administrator,
                Description = input.Description,
                General = input.General,
                AdmPageProfiles = pageProfiles,
                AdmUserProfiles = userProfiles
            };

            if (id != null)
            {
                obj.Id = (long)id;
            }

            return obj;
        }

        public async Task<AdmProfilePayload> AdmProfileInsertAsync(AdmProfileInput input)
        {
            var obj = SetObj(null, input);

            await this.service.Insert(obj);

            return new AdmProfilePayload(obj);
        }

        public async Task<AdmProfilePayload> AdmProfileUpdateAsync(long id,
            AdmProfileInput input)
        {
            var obj = SetObj(id, input);

            await this.service.Update(obj.Id, obj);

            return new AdmProfilePayload(obj);
        }

        public async Task<bool> AdmProfileDeleteAsync(long id)
        {
            var ret = await this.service.Delete(id);

            return ret;
        }
    }
}

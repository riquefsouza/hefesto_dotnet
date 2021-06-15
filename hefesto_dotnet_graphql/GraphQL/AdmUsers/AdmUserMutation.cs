using System.Linq;
using System.Threading.Tasks;
using hefesto.admin.Models;
using hefesto.admin.Services;

namespace hefesto_dotnet_graphql.GraphQL.AdmUsers
{
    public class AdmUserMutation : IAdmUserMutation
    {
        private readonly IAdmUserService service;

        public AdmUserMutation(IAdmUserService service)
        {
            this.service = service;
        }

        private AdmUser SetObj(long? id, AdmUserInput input)
        {
            var userProfiles = input.admIdProfiles
                .Select(userId => new AdmUserProfile(userId, (long)id))
                .ToList<AdmUserProfile>();

            var obj = new AdmUser
            {
                Active = input.Active,
                Email = input.Email,
                Login = input.Login,
                Name = input.Name,
                Password = input.Password,
                AdmUserProfiles = userProfiles
            };

            if (id != null)
            {
                obj.Id = (long)id;
            }

            return obj;
        }

        public async Task<AdmUserPayload> AdmUserInsertAsync(AdmUserInput input)
        {
            var obj = SetObj(null, input);

            await this.service.Insert(obj);

            return new AdmUserPayload(obj);
        }

        public async Task<AdmUserPayload> AdmUserUpdateAsync(long id,
            AdmUserInput input)
        {
            var obj = SetObj(id, input);

            await this.service.Update(obj.Id, obj);

            return new AdmUserPayload(obj);
        }

        public async Task<bool> AdmUserDeleteAsync(long id)
        {
            var ret = await this.service.Delete(id);

            return ret;
        }
    }
}

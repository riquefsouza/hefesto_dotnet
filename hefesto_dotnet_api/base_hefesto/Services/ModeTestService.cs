using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using hefesto.admin.VO;
using hefesto.admin.Services;
using hefesto.base_hefesto.Util;

namespace hefesto.base_hefesto.Services
{
    public class ModeTestService : IModeTestService
    {
        public static string TEST_PASSWORD = "dfkdfsldkhf";

        private readonly IAdmParameterService admParameterService;
        
        private readonly IAdmProfileService profileService;

        private readonly IAdmUserService userService;

        public ModeTestService(IAdmParameterService admParameterService, IAdmProfileService profileService,
            IAdmUserService userService)
        {
            this.admParameterService = admParameterService;
            this.profileService = profileService;
            this.userService = userService;
        }

        private JSONListConverter<ModeTestVO> conv;

        public ModeTestVO Load(String login)
        {
            List<ModeTestVO> lista = FindAll();
            return lista.Where(item => login.Equals(item.Login)).FirstOrDefault();
        }

        public List<ModeTestVO> FindAll()
        {
            List<ModeTestVO> lista = new List<ModeTestVO>();
            string valor;

            try
            {
                valor = admParameterService.GetValueByCode("MODO_TESTE");
            }
            catch (Exception)
            {
                valor = "";
            }

            if (valor != null && valor.Length > 0)
            {
                conv = new JSONListConverter<ModeTestVO>();
                lista = conv.JsonToList(valor);
            }
            return lista;
        }

        public async Task<AuthenticatedUserVO> Start(ISystemService systemService, 
            UserVO userVO, AuthenticatedUserVO authenticatedUser, bool usuarioLDAP)
        {
            authenticatedUser.ModeTest = false;
		
		    List<ModeTestVO> lista = FindAll();

            ModeTestVO mtvo = lista
                    .Where(vo => vo.Active.Equals(true))
                    .Where(vo => vo.Login.Equals(authenticatedUser.UserName))
                    .First();
		
		    if (mtvo != null){
			    authenticatedUser.ModeTest = true;
			    authenticatedUser.ModeTestLogin = authenticatedUser.UserName;
			
			    string svirtual = mtvo.LoginVirtual;			
			    if (svirtual.Length > 0) {
				    return await MountAuthenticatedUser(systemService, userVO, authenticatedUser, usuarioLDAP);
                }
            }

            return authenticatedUser;
	    }

        public async Task<AuthenticatedUserVO> MountAuthenticatedUser(ISystemService systemService,
            UserVO user, AuthenticatedUserVO authenticatedUser, bool usuarioLDAP)
        {

            authenticatedUser.UserName = user.Login;
            authenticatedUser.DisplayName = user.Name;
            authenticatedUser.Email = user.Email;
            authenticatedUser.ListPermission = await profileService.GetPermission(authenticatedUser);

            if (authenticatedUser.ListPermission.Count > 0)
            {

                List<long> listaIdPerfis = new List<long>();
                foreach (PermissionVO permissao in authenticatedUser.ListPermission)
                {
                    listaIdPerfis.Add(permissao.Profile.Id);
                }

                authenticatedUser.ListMenus =
                        systemService.FindMenuParentByProfile(listaIdPerfis);

                authenticatedUser.ListAdminMenus =
                        systemService.FindAdminMenuParentByProfile(listaIdPerfis);

            }
            else
            {
                throw new Exception("Usuário sem perfil associado!");
            }

            if (authenticatedUser.ModeTest)
            {
                authenticatedUser.ModeTestLoginVirtual = user.Login;
            }

            return authenticatedUser;
        }

        public bool EnableTestPassword()
        {
            bool habilitar = false;
            string valor;
            try
            {
                valor = admParameterService.GetValueByCode("HABILITAR_SENHA_TESTE");
                habilitar = Boolean.Parse(valor);
            }
            catch (Exception)
            {
                habilitar = false;
            }

            return habilitar;
        }

        public string SHA512(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            using (var hash = System.Security.Cryptography.SHA512.Create())
            {
                var hashedInputBytes = hash.ComputeHash(bytes);

                var strHash = BitConverter.ToString(hashedInputBytes).Replace("-", "");

                // Convert to text
                // StringBuilder Capacity is 128, because 512 bits / 8 bits in byte * 2 symbols for byte 
                /*
                var hashedInputStringBuilder = new StringBuilder(128);
                foreach (var b in hashedInputBytes)
                    hashedInputStringBuilder.Append(b.ToString("X2"));
                return hashedInputStringBuilder.ToString();
                */
                return strHash;
            }
        }

        public UserVO AutenticarViaSenhaTeste(string login, string password)
        {
            UserVO userVO = new UserVO();
            if (login.Length > 0 && password.Length > 0)
            {
                string csenha = this.SHA512(ModeTestService.TEST_PASSWORD);

                if (password.Equals(csenha))
                {
                    userVO = userService.FindByLikeEmail(login).First();
                }
            }
            return userVO;
        }


    }
}

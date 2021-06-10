using System;
using System.Collections.Generic;
using System.Linq;
using hefesto.admin.VO;
using System.Threading.Tasks;

namespace hefesto.base_hefesto.Services
{
    public interface IModeTestService
    {
        Task<AuthenticatedUserVO> Start(ISystemService systemService, 
            UserVO userVO, AuthenticatedUserVO authenticatedUser, bool usuarioLDAP);
        Task<AuthenticatedUserVO> MountAuthenticatedUser(ISystemService systemService, 
            UserVO user, AuthenticatedUserVO authenticatedUser, bool usuarioLDAP);
    }
}

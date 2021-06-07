using System;
using System.Collections.Generic;
using System.Linq;
using hefesto.base_hefesto.Models;
using System.Threading.Tasks;

namespace hefesto.base_hefesto.Services
{
    public interface ISystemService
    {
        Task<List<MenuItemDTO>> MountMenuItem(List<long> listaIdProfile);
    }
}

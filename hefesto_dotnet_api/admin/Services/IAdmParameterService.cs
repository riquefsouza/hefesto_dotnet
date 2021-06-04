using System.Collections.Generic;
using hefesto.admin.Models;
using hefesto.base_hefesto.Pagination;
using System.Threading.Tasks;
using hefesto.base_hefesto.Services;

namespace hefesto.admin.Services
{
    public interface IAdmParameterService: IBaseCrud<AdmParameter, long>
    {
        void SetTransient(List<AdmParameter> list);
        void SetTransient(AdmParameter item);
    }
}
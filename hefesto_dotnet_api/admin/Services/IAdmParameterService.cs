using System.Collections.Generic;
using hefesto.admin.Models;
using hefesto.base_hefesto.Services;
using System.Threading.Tasks;

namespace hefesto.admin.Services
{
    public interface IAdmParameterService: IBaseCrud<AdmParameter, long>
    {
        void SetTransient(List<AdmParameter> list);
        void SetTransient(AdmParameter item);

        string GetValueByCode(string scode);
    }
}
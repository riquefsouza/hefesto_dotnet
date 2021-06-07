using System.Collections.Generic;
using hefesto.admin.Models;
using hefesto.base_hefesto.Services;

namespace hefesto.admin.Services
{
    public interface IAdmPageService : IBaseCrud<AdmPage, long>
    {
        void SetTransient(List<AdmPage> list);
        void SetTransient(AdmPage item);
    }
}
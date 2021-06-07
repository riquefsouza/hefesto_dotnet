using System.Linq;
using System.Collections.Generic;
using hefesto.admin.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using hefesto.base_hefesto.Pagination;
using hefesto.base_hefesto.Services;

namespace hefesto.admin.Services
{
    public class AdmMenuService : IAdmMenuService
    {
        private readonly dbhefestoContext _context;

        private readonly IUriService _uriService;

        public AdmMenuService(dbhefestoContext context, IUriService uriService)
        {
            _context = context;
            _uriService = uriService;
        }

        public void SetTransientWithoutSubMenus(List<AdmMenu> list)
        {
            foreach (var item in list) {
                SetTransientSubMenus(item, null);
            }
        }

        public void SetTransient(List<AdmMenu> list)
        {
            foreach (var item in list) {
                SetTransient(item);
            }
        }

        public void SetTransientSubMenus(AdmMenu item, List<AdmMenu> subMenus)
        {
            item.AdmPage = _context.AdmPages.Find(item.IdPage);
            item.AdmMenuParent = _context.AdmMenus.Find(item.IdMenuParent); 
            item.Url = item.AdmPage != null ? item.AdmPage.Url : null;
            item.InverseAdmMenuParent = subMenus;
        }
        public void SetTransient(AdmMenu item)
        {
            SetTransientSubMenus(item, findByIdMenuParent(item.Id));
        }

        public List<AdmMenu> findByIdMenuParent(long? idMenuParent){
            if (idMenuParent!=null) {
                var query = _context.AdmMenus.AsQueryable();
                query = _context.AdmMenus.Where(adm => adm.IdMenuParent == idMenuParent);
                List<AdmMenu> lista = query.ToList();
                //SetTransientWithoutSubMenus(lista);
                return lista;
            }
            return new List<AdmMenu>();
        }

        public async Task<BasePaged<AdmMenu>> GetPage(string route, PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.pageNumber, filter.size, filter.sort,
                filter.columnOrder, filter.columnTitle);

            var pagedData = await _context.AdmMenus
                .Skip((validFilter.pageNumber - 1) * validFilter.size)
                .Take(validFilter.size)
                .ToListAsync();
            var totalRecords = await _context.AdmMenus.CountAsync();
            this.SetTransient(pagedData);

            return new BasePaged<AdmMenu>(pagedData,
                BasePaging.of(validFilter, totalRecords, _uriService, route));
        }

        public async Task<List<AdmMenu>> FindAll()
        {
            var listObj = await _context.AdmMenus.ToListAsync();
            this.SetTransient(listObj);
            return listObj;
        }

        public async Task<AdmMenu> FindById(long? id)
        {
            var obj = await _context.AdmMenus.FindAsync(id);

            if (obj != null)
            {
                this.SetTransient(obj);
            }

            return obj;
        }

        public async Task<bool> Update(long id, AdmMenu obj)
        {
            _context.Entry(obj).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!this.Exists(id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }

            return true;
        }

        public async Task<AdmMenu> Insert(AdmMenu obj)
        {
            obj.Id = this.GetNextSequenceValue();

            _context.AdmMenus.Add(obj);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (this.Exists(obj.Id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }

            return obj;
        }

        public async Task<bool> Delete(long id)
        {
            var obj = await _context.AdmMenus.FindAsync(id);
            if (obj == null)
            {
                return false;
            }

            _context.AdmMenus.Remove(obj);
            await _context.SaveChangesAsync();

            return true;
        }

        public bool Exists(long id)
        {
            return _context.AdmMenus.Any(e => e.Id == id);
        }

        private long GetNextSequenceValue()
        {
            var rawQuery = _context.Set<SequenceValue>().FromSqlRaw("select nextval('public.adm_menu_seq') as Value;");
            var nextVal = rawQuery.AsEnumerable().First().Value;

            return nextVal;
        }
    }
}
using hefesto.base_hefesto.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace hefesto.base_hefesto.Services
{
    public interface IBaseCrud<T, I>
    {

		Task<BasePaged<T>> GetPage(string route, PaginationFilter filter);

		Task<List<T>> FindAll();

		Task<T> FindById(long? id);		

		Task<bool> Update(I id, T obj);

		Task<T> Insert(T obj);

		Task<bool> Delete(I id);

		bool Exists(I id);
	}
}

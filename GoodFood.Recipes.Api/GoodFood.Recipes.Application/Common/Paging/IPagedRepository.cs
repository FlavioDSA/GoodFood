using System.Threading.Tasks;

namespace GoodFood.Recipes.Application.Common.Paging
{
    public interface IPagedRepository<T>
    {
        Task<PagedResult<T>> GetAsync(string userId, PagedRequest pagedRequest);
    }
}
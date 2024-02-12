using storeBack.Models;
using storeBack.ViewModels;

namespace storeBack.Services.Articulos
{
    public interface IArticuloService
    {
        Task<TableResponse<Articulo>> getAllArticlesAsync(int offset, int limit);
        Task<Articulo> getArticleByIdAsync(int id);
        Task<Articulo> CreateArticleAsync(ArticuloDto article);
        Task UpdateArticleAsync(int id, ArticuloDto article);
        Task DeleteArticleAsync(int id);
    }
}

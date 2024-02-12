using storeBack.Models;
using storeBack.Services.ArticulosCliente;
using storeBack.ViewModels;

namespace storeBack.Services.ArticulosTienda
{
    public interface IArticuloTiendaService
    {
        Task<TableResponse<ArticuloTiendaTableDto>> getAllAsync(int offset, int limit);
        Task<ArticuloTiendaTableDto> getByIdAsync(int id);
        Task<ArticuloTienda> CreateAsync(ArticuloTiendaDto relation);
        Task UpdateAsync(int id, ArticuloTiendaDto relation);
        Task DeleteAsync(int id);
    }
}

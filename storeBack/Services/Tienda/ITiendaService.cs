using storeBack.ViewModels;

namespace storeBack.Services.Tienda
{
    public interface ITiendaService
    {
        Task<TableResponse<TiendaDto>> getAllStoreAsync(int offset, int limit);
        Task<Models.Tienda> getStoreByIdAsync(int id);
        Task CreateStoreAsync(Models.Tienda store);
        Task UpdateStoreAsync(int id, UpdateTiendaDto store);
        Task DeleteStoreAsync(int id);
    }
}

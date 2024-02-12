using Microsoft.EntityFrameworkCore;
using storeBack.ViewModels;

namespace storeBack.Services.Tienda
{
    public class TiendaService(ApplicationDbContext context) : ITiendaService
    {
        private readonly ApplicationDbContext _context = context;

        public async Task CreateStoreAsync(Models.Tienda store)
        {
            _context.Tienda.Add(store);
            await  _context.SaveChangesAsync();
        }

        public async Task DeleteStoreAsync(int id)
        {
            Models.Tienda store = await _context.Tienda.FindAsync(id);
            if (store != null)
            {
                _context.Tienda.Remove(store);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<TableResponse<TiendaDto>> getAllStoreAsync(int offset, int limit)
        {
            var totalStores = await _context.Tienda.CountAsync();

            var stores = await _context
                .Tienda.OrderBy(s => s.Id)
                .Skip(offset)
                .Take(limit)
                .Select(s => new TiendaDto
                {
                    Id = s.Id,
                    Sucursal = s.Sucursal,
                    Direccion = s.Direccion
                })
                .ToListAsync();

            var response = new TableResponse<TiendaDto>
            {
                Count = totalStores,
                Results = stores
            };

            return response;
        }

        public async Task<Models.Tienda> getStoreByIdAsync(int id)
        {
            var store = await _context.Tienda.FindAsync(id);
            return store;
        }

        public async Task UpdateStoreAsync(int id, UpdateTiendaDto store)
        {
            var storeInfo = await _context.Tienda.FindAsync(id);

            storeInfo.Sucursal = store.Sucursal;
            storeInfo.Direccion = store.Direccion;


            await _context.SaveChangesAsync();
        }
    }
}

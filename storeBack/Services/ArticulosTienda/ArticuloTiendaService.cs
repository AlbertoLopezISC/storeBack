using Microsoft.EntityFrameworkCore;
using storeBack.Models;
using storeBack.Services.ArticulosCliente;
using storeBack.ViewModels;

namespace storeBack.Services.ArticulosTienda
{
    public class ArticuloTiendaService(ApplicationDbContext context) : IArticuloTiendaService
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<ArticuloTienda> CreateAsync(ArticuloTiendaDto relation)
        {
            var relationInfo = new ArticuloTienda
            {
                ArticuloId = relation.ArticuloId,
                TiendaId = relation.TiendaId
            };
            _context.ArticuloTienda.Add(relationInfo);
            await _context.SaveChangesAsync();
            return relationInfo;
        }

        public async Task DeleteAsync(int id)
        {
            ArticuloTienda relation = await _context.ArticuloTienda.FindAsync(id);
            if (relation != null)
            {
                _context.ArticuloTienda.Remove(relation);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<TableResponse<ArticuloTiendaTableDto>> getAllAsync(int offset, int limit)
        {
            var total = await _context.ArticuloTienda.CountAsync();

            var relations = await _context
                .ArticuloTienda
                .Include(at => at.Articulo)
                .Include(at => at.Tienda)
                .OrderBy(at => at.Id)
                .Skip(offset)
                .Take(limit)
                .Select(at => new ArticuloTiendaTableDto
                {
                    Id = at.Id,
                    ArticuloId = at.ArticuloId,
                    TiendaId = at.TiendaId,
                })
                .ToListAsync();

            var response = new TableResponse<ArticuloTiendaTableDto>
            {
                Count = total,
                Results = relations
            };

            return response;
        }

        public async Task<ArticuloTiendaTableDto> getByIdAsync(int id)
        {
            var relation = await _context.ArticuloTienda.Select(at => new ArticuloTiendaTableDto
            {
                Id = at.Id,
                ArticuloId = at.ArticuloId,
                TiendaId = at.TiendaId
            }).FirstOrDefaultAsync(at => at.Id == id);
            return relation;
        }

        public async Task UpdateAsync(int id, ArticuloTiendaDto relation)
        {
            var relationInfo = await _context.ArticuloTienda.FindAsync(id);

            relationInfo.TiendaId = relation.TiendaId;
            relationInfo.ArticuloId = relation.ArticuloId;

            await _context.SaveChangesAsync();
        }
    }
}

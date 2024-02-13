using Microsoft.EntityFrameworkCore;
using storeBack.Models;
using storeBack.Services.Articulos;
using storeBack.Services.Cliente;
using storeBack.ViewModels;

namespace storeBack.Services.ArticulosCliente
{
    public class ArticuloClienteService(ApplicationDbContext context) : IArticuloClienteService
    {

        private readonly ApplicationDbContext _context = context;

        public async Task<ArticuloCliente> CreateAsync(ArticuloClienteDto relation)
        {

            var relationInfo = _context.ArticuloCliente
                .FirstOrDefault(ac => ac.ArticuloId == relation.ArticuloId && ac.ClienteId == relation.ClienteId);
            var articulo = _context.Articulos.Find(relation.ArticuloId);

            if ((relation.Cantidad + relation?.Cantidad ?? 0) > articulo.Stock) {
                throw new InvalidOperationException($"No hay suficiente stock disponible para el artículo con ID {articulo.Id}");
            }

            if(relationInfo != null)
            {
                relationInfo.Cantidad += relation.Cantidad;
                await _context.SaveChangesAsync();
                return relationInfo;
            }

            var newRelation = new ArticuloCliente
            {
                ArticuloId = relation.ArticuloId,
                ClienteId = relation.ClienteId,
                Cantidad = relation.Cantidad,
            };
            _context.ArticuloCliente.Add(newRelation);
            await _context.SaveChangesAsync();
            return newRelation;
        }

        public async Task DeleteAsync(int id)
        {
            ArticuloCliente relation = await _context.ArticuloCliente.FindAsync(id);
            if (relation != null)
            {
                _context.ArticuloCliente.Remove(relation);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteByClientIdAsync(int clientId)
        {
            var relations = await _context.ArticuloCliente
                .Where(ac => ac.ClienteId == clientId)
                .ToListAsync();
            if (relations != null && relations.Any())
            {
                _context.ArticuloCliente.RemoveRange(relations);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<ArticuloCliente>> GetByClientIdAsync(int clientId)
        {
            var relations = await _context.ArticuloCliente
                .Include(ac => ac.Articulo)
                .Where(ac => ac.ClienteId == clientId)
                .ToListAsync();
            return relations;
        }

        public async Task<TableResponse<ArticuloClienteTableDto>> getAllAsync(int offset, int limit)
        {
            var total = await _context.ArticuloCliente.CountAsync();

            var relations = await _context
                .ArticuloCliente
                .OrderBy(ac => ac.Id)
                .Skip(offset)
                .Take(limit)
                .Select(ac => new ArticuloClienteTableDto
                {
                    Id = ac.Id,
                    ClienteId = ac.ClienteId,
                    ArticuloId = ac.ArticuloId,
                    Cantidad = ac.Cantidad,
                })
                .ToListAsync();

            var response = new TableResponse<ArticuloClienteTableDto>
            {
                Count = total,
                Results = relations
            };

            return response;
        }

        public async Task<ArticuloClienteTableDto> getByIdAsync(int id)
        {
            var relation = await _context.ArticuloCliente.Select(ac => new ArticuloClienteTableDto
            {
                Id = ac.Id,
                ArticuloId = ac.ArticuloId,
                ClienteId = ac.ClienteId,
                Cantidad = ac.Cantidad
            }).FirstOrDefaultAsync(ac => ac.Id == id);
            return relation;
        }

        public async Task UpdateAsync(int id, ArticuloClienteDto relation)
        {
            var relationInfo = await _context.ArticuloCliente.FindAsync(id);

            relationInfo.ClienteId = relation.ClienteId;
            relationInfo.ArticuloId = relation.ArticuloId;
            relationInfo.Cantidad = relation.Cantidad;

            await _context.SaveChangesAsync();
        }
    }
}

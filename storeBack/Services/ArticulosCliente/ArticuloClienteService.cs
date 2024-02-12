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
                ClienteId = ac.ClienteId
            }).FirstOrDefaultAsync(ac => ac.Id == id);
            return relation;
        }

        public async Task UpdateAsync(int id, ArticuloClienteDto relation)
        {
            var relationInfo = await _context.ArticuloCliente.FindAsync(id);

            relationInfo.ClienteId = relation.ClienteId;
            relationInfo.ArticuloId = relation.ArticuloId;

            await _context.SaveChangesAsync();
        }
    }
}

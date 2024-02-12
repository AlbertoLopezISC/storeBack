using Microsoft.EntityFrameworkCore;
using storeBack.Models;
using storeBack.Services.Tienda;
using storeBack.ViewModels;
using System;

namespace storeBack.Services.Articulos
{
    public class ArticuloService(ApplicationDbContext context) : IArticuloService
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Articulo> CreateArticleAsync(ArticuloDto article)
        {
            var data = new Articulo
            {
                Nombre = article.Nombre,
                Descripcion = article.Descripcion,
                Precio = article.Precio,
                ImgUrl = article.ImgUrl,
                Stock = article.Stock
            };

            _context.Articulos.Add(data);
            await _context.SaveChangesAsync();
            return data;
        }

        public async Task DeleteArticleAsync(int id)
        {
            Models.Articulo article = await _context.Articulos.FindAsync(id);
            if (article != null)
            {
                _context.Articulos.Remove(article);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<TableResponse<Articulo>> getAllArticlesAsync(int offset, int limit)
        {
            var total= await _context.Articulos.CountAsync();

            var articles = await _context
                .Articulos.OrderBy(s => s.Id)
                .Skip(offset)
                .Take(limit)
                .Select(a => new Articulo
                {
                    Id = a.Id,
                    Nombre = a.Nombre,
                    Descripcion = a.Descripcion,
                    Precio = a.Precio,
                    ImgUrl = a.ImgUrl,
                    Stock = a.Stock

                })
                .ToListAsync();

            var response = new TableResponse<Articulo>
            {
                Count = total,
                Results = articles
            };

            return response;
        }

        public async Task<Articulo> getArticleByIdAsync(int id)
        {
            var article = await _context.Articulos.FindAsync(id);
            return article;
        }

        public async Task UpdateArticleAsync(int id, ArticuloDto article)
        {
            var articleInfo = await _context.Articulos.FindAsync(id);

            articleInfo.Nombre = article.Nombre;
            articleInfo.Descripcion = article.Descripcion;
            articleInfo.Precio = article.Precio;
            articleInfo.ImgUrl = article.ImgUrl;
            articleInfo.Stock = article.Stock;

            await _context.SaveChangesAsync();
        }


    }
}

﻿using storeBack.Models;
using storeBack.ViewModels;

namespace storeBack.Services.ArticulosCliente
{
    public interface IArticuloClienteService
    {
        Task<TableResponse<ArticuloClienteTableDto>> getAllAsync(int offset, int limit);
        Task<ArticuloClienteTableDto> getByIdAsync(int id);
        Task<ArticuloCliente> CreateAsync(ArticuloClienteDto relation);
        Task UpdateAsync(int id, ArticuloClienteDto relation);
        Task DeleteAsync(int id);
    }
}

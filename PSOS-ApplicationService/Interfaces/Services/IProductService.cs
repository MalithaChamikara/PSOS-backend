using PSOS_BusinessObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSOS_ApplicationService.Interfaces.Services
{
    public interface IProductService
    {
        Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken ct = default);
        Task<IReadOnlyList<Product>> GetLowStockAsync(CancellationToken ct = default);
    }
}

using PSOS_ApplicationService.Interfaces.Repositories;
using PSOS_ApplicationService.Interfaces.Services;
using PSOS_BusinessObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSOS_ApplicationService.Services
{
    public class ProductService : IProductService
    {

        private readonly IProductRepository _productsRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productsRepository = productRepository;
        }
        public const int LowStockThreshold = 5;

        public Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken ct = default)
            => _productsRepository.GetAllAsync(ct);

        public Task<IReadOnlyList<Product>> GetLowStockAsync(CancellationToken ct = default)
            => _productsRepository.GetLowStockAsync(LowStockThreshold, ct);
    }
}

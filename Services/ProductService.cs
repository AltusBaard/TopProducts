using Contracts.Interfaces;
using Domain.Models.Order;
using Domain.Models.Result;
using LanguageExt;
using Microsoft.Extensions.Logging;

namespace Services
{
    public class ProductService : IProductService
    {
        private readonly ILogger<IProductRepository> _logger;
        private readonly IProductRepository _productRepository;

        public ProductService(ILogger<IProductRepository> logger, IProductRepository productRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
        }

        public async Task<Option<Result>> ProcessOrder(OrderItem[] orders)
        {
            var productsOption = await _productRepository.GetAll().ConfigureAwait(false);

            return productsOption.Match(
                products =>
                {
                    var order = new Order(orders, products.Select(x => x).ToArray());
                    order.CalculateTopProducts();
                    return new Result
                    {
                        Daily = order.DailyTop,
                        Top = order.ThreeDayTop
                    };
                },
                () =>
                {
                    _logger.LogError("Could not load products from repository");
                    return Option<Result>.None;
                });
        }
    }
}

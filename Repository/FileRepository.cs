using Contracts.Interfaces;
using Domain.Models.Product;
using LanguageExt;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Text;

namespace Repository
{
    public class FileRepository : IProductRepository
    {
        private readonly ILogger<FileRepository> _logger;

        public FileRepository(ILogger<FileRepository> logger)
        {
            _logger = logger;
        }

        public async Task<Option<ReadOnlyCollection<ProductItem>>> GetAll()
        {
            try
            {
                var product = Encoding.UTF8.GetString(Properties.Resources.Products, 0, Properties.Resources.Products.Length);
                if (!string.IsNullOrEmpty(product))
                {
                    var products = JsonConvert.DeserializeObject<ProductItem[]>(product);
                    if (products != null)
                        return new ReadOnlyCollection<ProductItem>(products);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occured while trying to load data.");
                return Option<ReadOnlyCollection<ProductItem>>.None;
            }
            return Option<ReadOnlyCollection<ProductItem>>.None;
        }
    }
}

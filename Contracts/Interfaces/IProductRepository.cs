using Domain.Models.Product;
using LanguageExt;
using System.Collections.ObjectModel;

namespace Contracts.Interfaces
{
    public interface IProductRepository
    {
        Task<Option<ReadOnlyCollection<ProductItem>>> GetAll();
    }
}

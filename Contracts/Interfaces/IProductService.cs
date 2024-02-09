using Domain.Models.Order;
using Domain.Models.Result;
using LanguageExt;

namespace Contracts.Interfaces
{
    public interface IProductService
    {
        Task<Option<Result>> ProcessOrder(OrderItem[] orders);
    }
}
using Domain.Models.Product;

namespace Domain.Models.Order
{
    public class Order
    {
        private OrderItem[] _orderItems;
        private ProductItem[] _productItems;

        public string ThreeDayTop { get; set; } = default!;
        public List<string> DailyTop { get; set; } = [];

        private Order() { }

        public Order(OrderItem[] orderItems, ProductItem[] productItems)
        {
            if (orderItems == null || orderItems.Length == 0)
            {
                throw new ArgumentNullException(nameof(orderItems));
            }
            if (productItems == null || productItems.Length == 0)
            {
                throw new ArgumentNullException(nameof(productItems));
            }
            var receivedIds = orderItems.SelectMany(x => x.Entries).Select(x => x.Id);
            var availableIds = productItems.Select(x => x.Id);
            var invalidIds = receivedIds.Except(availableIds);
            if (invalidIds.Any())
            {
                throw new ArgumentException($"An id was received that is not available from stock list: {string.Join(',', invalidIds)}");
            }

            _orderItems = orderItems;
            _productItems = productItems;
        }

        public void CalculateTopProducts()
        {
            var cancelledOrders = _orderItems.Where(x => x.Status.Equals(Constants.Cancelled, StringComparison.OrdinalIgnoreCase));
            var remainingOrders = cancelledOrders.Any() ? _orderItems.Where(x => cancelledOrders.Any(c => c.OrderId != x.OrderId)) : _orderItems;

            var dailyOrders = remainingOrders.GroupBy(x => x.Date);

            var threeDayTracking = new List<(string id, int count)>();

            foreach (var dailyOrder in dailyOrders)
            {
                var dailyIdsOrdered = dailyOrder.SelectMany(x => x.Entries).Select(x => x.Id);
                var dailyCountById = dailyIdsOrdered.GroupBy(x => x).Select(x => new { id = x, count = x.Count() });

                threeDayTracking.AddRange(dailyCountById.Select(x => (x.id.Key, x.count)));

                var dailyHighest = dailyCountById.Where(x => x.count == dailyCountById.Max(y => y.count));
                var dailyHighestName = _productItems.Where(x => dailyHighest.Any(y => y.id.Key == x.Id)).OrderBy(x => x.Name).First().Name;

                DailyTop.Add($"{dailyOrder.Key}: '{dailyHighestName}'");
            }

            var totalsPerItem = threeDayTracking.GroupBy(x => x.id).Select(x => new { id = x.First().id, sum = x.Sum(c => c.count) });
            var threeDayHighestSum = totalsPerItem.Where(x => x.sum == totalsPerItem.Max(x => x.sum)).First();
            var threeDayHighestName = _productItems.Where(x => x.Id == threeDayHighestSum.id).First();

            ThreeDayTop = $"Last 3 days: {threeDayHighestName.Name}";
        }
    }
}

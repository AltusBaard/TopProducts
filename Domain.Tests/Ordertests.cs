using Domain.Models.Order;
using Domain.Models.Product;

namespace Domain.Tests
{
    public class Ordertests
    {
        [Test]
        public void TryingToInstantiateAnOrderWithoutValidDataCausesException()
        {
            Assert.Throws<ArgumentNullException>(() => { new Order(null, null); });
            Assert.Throws<ArgumentNullException>(() => { new Order(null, []); });
            Assert.Throws<ArgumentNullException>(() => { new Order([], null); });
            Assert.Throws<ArgumentNullException>(() => { new Order([], []); });

            Assert.Throws<ArgumentNullException>(() => { new Order([new OrderItem { }], []); });
            Assert.Throws<ArgumentNullException>(() => { new Order([], [new ProductItem { }]); });

            Assert.Throws<ArgumentException>(() => {
                new Order(
                [
                    new OrderItem
                    {
                        CustomerId = "01",
                        Date = "19/01/2020",
                        OrderId = "01",
                        Status = Constants.Completed,
                        Entries = new List<Entry>
                        {
                            new Entry
                            {
                                Id = "5",
                                Quantity = 1
                            }
                        }
                    }
                ],
                [
                    new ProductItem
                    {
                        Id = "1",
                        Name = "My product"
                    }
                ]); });
        }

        [Test]
        public void CalculatingTopProductsReturnsCorrectValues()
        {
            var order = new Order(
                [
                    new OrderItem
                    {
                        CustomerId = "01",
                        Date = "19/01/2020",
                        OrderId = "01",
                        Status = Constants.Completed,
                        Entries = new List<Entry> 
                        { 
                            new Entry 
                            { 
                                Id = "1",
                                Quantity = 1
                            } 
                        }
                    }
                ],
                [
                    new ProductItem
                    {
                        Id = "1",
                        Name = "My product"
                    }
                ]);

            order.CalculateTopProducts();

            Assert.That(order.ThreeDayTop, Is.EqualTo("Last 3 days: My product"));
            Assert.That(order.DailyTop.Count, Is.EqualTo(1));
        }
    }
}
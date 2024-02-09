using Newtonsoft.Json;

namespace Domain.Models.Order
{
    public class OrderItem
    {
        [JsonProperty("orderId")]
        public string OrderId { get; set; } = string.Empty;
        [JsonProperty("customerId")]
        public string CustomerId { get; set; } = string.Empty;
        [JsonProperty("entries")]
        public List<Entry> Entries { get; set; } = [];
        [JsonProperty("date")]
        public string Date { get; set; } = string.Empty;
        [JsonProperty("status")] 
        public string Status { get; set; } = string.Empty;
    }
}

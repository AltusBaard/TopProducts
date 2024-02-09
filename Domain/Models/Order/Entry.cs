using Newtonsoft.Json;

namespace Domain.Models.Order
{
    public class Entry
    {
        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;
        [JsonProperty("quantity")]
        public int Quantity { get; set; }
    }
}

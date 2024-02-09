using Newtonsoft.Json;

namespace Domain.Models.Product
{
    public class ProductItem
    {
        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;
    }
}

using Newtonsoft.Json;

namespace DownloadingExcelFile.Data
{
    public class Fruit
    {
        [JsonProperty("name")] public string name { get; set; }
        [JsonProperty("id")] public int id { get; set; }
        [JsonProperty("family")] public string family { get; set; }
        [JsonProperty("genus")] public string genus { get; set; }
        [JsonProperty("order")] public string order { get; set; }
        [JsonProperty("nutritions")] public nutritions nutritions { get; set; }
    }
    public class nutritions
    {
        [JsonProperty("carbohydrates")] public decimal carbohydrates { get; set; }
        [JsonProperty("protein")] public decimal protein { get; set; }
        [JsonProperty("fat")] public decimal fat { get; set; }
        [JsonProperty("calories")] public decimal calories { get; set; }
        [JsonProperty("sugar")] public decimal sugar { get; set; }
    }
}

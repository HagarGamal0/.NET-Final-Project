using System.Text.Json.Serialization;

namespace PickGo_backend.DTOs.GraphHopper
{
    public class GraphHopperPathDto
    {
        [JsonPropertyName("distance")]

        public double Distance { get; set; } // meters
        [JsonPropertyName("time")]
        public long Time { get; set; }       // milliseconds
}

}

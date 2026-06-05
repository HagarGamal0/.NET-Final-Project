
using PickGo_backend.DTOs.GraphHopper;
using System.Text.Json.Serialization;

public class GraphHopperResponseDto
    {
    [JsonPropertyName("paths")]
    public List<GraphHopperPathDto> Paths { get; set; } = new();
}


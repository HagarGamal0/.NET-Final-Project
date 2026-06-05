using PickGo_backend.Services;
using System.Text.Json;


    public class GraphHopperService : IGraphHopperService
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _http;

        public GraphHopperService(IConfiguration config, HttpClient http)
        {
            _config = config;
            _http = http;
        }

        public async Task<(double distanceKm, double etaMinutes)> GetRouteAsync(
            double fromLat, double fromLng, double toLat, double toLng, string vehicle)
        {
            string apiKey = _config["GraphHopper:ApiKey"];
            if (string.IsNullOrEmpty(apiKey))
                throw new Exception("GraphHopper API key is missing.");

            string url = $"{_config["GraphHopper:BaseUrl"]}?point={fromLat},{fromLng}&point={toLat},{toLng}&vehicle={vehicle}&key={apiKey}";

            var response = await _http.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                string err = await response.Content.ReadAsStringAsync();
                throw new Exception($"GraphHopper API error: {err}");
            }

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<GraphHopperResponseDto>(json);

            if (data?.Paths == null || !data.Paths.Any())
                throw new Exception("GraphHopper returned no route.");

            var path = data.Paths.First();
            double distanceKm = path.Distance / 1000.0;
            double etaMinutes = path.Time / 60000.0;

            return (distanceKm, etaMinutes);
        }
    }


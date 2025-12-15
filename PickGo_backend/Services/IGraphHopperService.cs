namespace PickGo_backend.Services
{
    public interface IGraphHopperService
    {
        Task<(double distanceKm, double etaMinutes)> GetRouteAsync(
            double fromLat, double fromLng,
            double toLat, double toLng,
            string vehicle);
    }
}

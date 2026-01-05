using PickGo_backend.Models;
using PickGo_backend.Models.Lynx;
using PickGo_backend; // For UnitOfWork
using System.Text;
using System.Text.Json;

namespace PickGo_backend.Services
{
    public class LynxTalismanService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly CourierMatchingService _matchingService;
        private readonly HttpClient _httpClient; 
        private const string OLLAMA_URL = "http://localhost:11434/api/generate";

        public LynxTalismanService(UnitOfWork unitOfWork, CourierMatchingService matchingService, HttpClient httpClient)
        {
            _unitOfWork = unitOfWork;
            _matchingService = matchingService;
            _httpClient = httpClient;
        }

        public async Task ExplainAssignmentAsync(int requestId, int selectedCourierId, string decisionSource)
        {
            try
            {
                // 1. Fetch Order Context
                var request = await _unitOfWork.RequestRepo.GetByIdAsync(requestId);
                if (request == null) return;

                // 2. Fetch Selected Courier
                var selectedCourier = await _unitOfWork.CourierRepo.GetByIdAsync(selectedCourierId);
                var selectedCourierUser = selectedCourier != null ? await _unitOfWork.UserRepo.GetByIdAsync(selectedCourier.UserId) : null;
                
                // 3. Re-run Ranking to get context
                var candidates = await _matchingService.GetRankedCouriersAsync(request.PickupLat, request.PickupLng);
                
                // 4. Construct Prompt Context
                var sb = new StringBuilder();
                sb.AppendLine("You are The Lynx Talisman, an observer of dispatch decisions.");
                sb.AppendLine($"Order ID: {requestId}. Decision Source: {decisionSource}.");
                sb.AppendLine($"Selected Courier: {selectedCourierUser?.UserName ?? "Unknown"} (ID: {selectedCourierId}, Vehicle: {selectedCourier?.VehicleType}).");
                
                // Find selected in candidates to get its metrics
                var selectedMetrics = candidates.FirstOrDefault(c => c.Courier.Id == selectedCourierId);
                if (selectedMetrics != null)
                {
                    sb.AppendLine($"Selected Metrics: Distance {selectedMetrics.DistanceKm}km, ETA {selectedMetrics.EtaMinutes}min.");
                }
                else
                {
                    sb.AppendLine("Selected Courier was NOT in the top recommendations (likely far away or offline at ranking time).");
                }

                sb.AppendLine("Top Candidates were:");
                foreach (var c in candidates.Take(3))
                {
                    sb.AppendLine($"- {c.Courier.User?.UserName} (ID: {c.Courier.Id}): {c.DistanceKm}km, {c.EtaMinutes}min, {c.Courier.VehicleType}");
                }

                sb.AppendLine("Task: Explain in one calm, factual sentence why the selected courier was chosen. If the selected courier was not the top rank, note that this was likely a manual preference.");

                // 5. Call AI (Fail-safe)
                string explanation = await CallLocalAI(sb.ToString());

                if (string.IsNullOrEmpty(explanation))
                {
                    // Fallback
                    if (selectedMetrics != null && candidates.FirstOrDefault()?.Courier.Id == selectedCourierId)
                    {
                        explanation = "System selected the optimal candidate based on proximity and ETA.";
                    }
                    else
                    {
                        explanation = "A variation from the standard ranking was observed. Specific operational factors may apply.";
                    }
                }

                // 6. Save Observation
                var observation = new AssignmentObservation
                {
                    RequestId = requestId,
                    CourierId = selectedCourierId,
                    DecisionSource = decisionSource,
                    Explanation = explanation,
                    Timestamp = DateTime.UtcNow
                };

                await _unitOfWork.AssignmentObservationRepo.AddAsync(observation);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                // Never block the flow
                Console.WriteLine($"Lynx Talisman Warning: Failed to observe assignment. {ex.Message}");
            }
        }

        private async Task<string> CallLocalAI(string prompt)
        {
            try
            {
                var payload = new
                {
                    model = "llama3", // or mistral, parameterize if needed
                    prompt = prompt,
                    stream = false
                };

                var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(OLLAMA_URL, content);
                
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var doc = JsonDocument.Parse(json);
                    if (doc.RootElement.TryGetProperty("response", out var resp))
                    {
                        return resp.GetString()?.Trim();
                    }
                }
            }
            catch 
            {
                // Ignore AI failures, return null to trigger fallback
            }
            return null;
        }
    }
}

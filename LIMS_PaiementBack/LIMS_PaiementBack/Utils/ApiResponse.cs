using System.Text.Json;
using System.Text.Json.Serialization;

namespace LIMS_PaiementBack.Utils
{
    public class ApiResponse
    {

        public void HandleResponse<T>()
        {
            if (this?.Data != null)
            {
                var jsonString = JsonSerializer.Serialize(this.Data);
                var departementDtos = JsonSerializer.Deserialize<T>(jsonString);
                if (departementDtos != null) this.Data = departementDtos;
            }

        }

        [JsonPropertyName("data")]
        public object? Data { get; set; }
        [JsonPropertyName("viewBag")]
        public Dictionary<string, object>? ViewBag { get; set; }
        [JsonPropertyName("message")]
        public string? Message { get; set; }
        [JsonPropertyName("isSuccess")]
        public bool IsSuccess { get; set; }
        [JsonPropertyName("statusCode")]
        public int StatusCode { get; set; }

        public static implicit operator Dictionary<object, object>(ApiResponse v)
        {
            throw new NotImplementedException();
        }
        // Add any other properties you need (e.g., errors, metadata)
    }
}

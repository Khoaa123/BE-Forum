using System.Text.Json;

namespace Be_Voz_Clone.src.Services.Common
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}

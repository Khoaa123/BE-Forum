using System.Text.Json;

namespace Be_Voz_Clone.src.Shared.Core.Common
{
    public class ResultMessage
    {
        public string? Type { get; set; }
        public string? Message { get; set; }

        public ResultMessage()
        {
        }

        public ResultMessage(string type, string message)
        {
            Type = type;
            Message = message;
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
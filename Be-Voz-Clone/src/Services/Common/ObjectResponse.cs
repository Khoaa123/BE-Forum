using System.Text.Json.Serialization;

namespace Be_Voz_Clone.src.Services.Common
{
    public class ObjectResponse<T> : BaseResponse where T : class
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public T? Data { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? TotalPages { get; set; }

    }
}

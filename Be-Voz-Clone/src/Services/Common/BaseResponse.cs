using Be_Voz_Clone.src.Shared.Core.Enums;

namespace Be_Voz_Clone.src.Services.Common
{
    public class BaseResponse
    {
        public ResponseCode StatusCode { get; set; } = ResponseCode.OK;

        public string? Message { get; set; }
    }
}

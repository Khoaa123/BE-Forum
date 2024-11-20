using Be_Voz_Clone.src.Shared.Core.Common;
using Be_Voz_Clone.src.Shared.Core.Enums;
using System.Text.Json.Serialization;

namespace Be_Voz_Clone.src.Services.Common
{
    public class BaseResponse
    {
        public ResponseCode StatusCode { get; set; } = ResponseCode.OK;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ResultMessage? Message { get; set; }

        public void AddMessage(string message)
        {
            Message = new ResultMessage("Success", message);
        }

        public void AddMessage(ResultMessage message)
        {
            Message = message;
        }

        public void AddError(string message)
        {
            StatusCode = ResponseCode.BADREQUEST;
            Message = new ResultMessage("Error", message);
        }

        public void AddError(ResultMessage message)
        {
            StatusCode = ResponseCode.BADREQUEST;
            Message = message;
        }
    }
}
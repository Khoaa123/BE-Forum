using System.ComponentModel.DataAnnotations;

namespace Be_Voz_Clone.src.Services.DTO.Account
{
    public class AccountLoginRequest
    {
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Be_Voz_Clone.src.Services.DTO.Account
{
    public class AccountRegisterRequest
    {
        public string UserName { get; set; }
        public string DisplayName { get; set; }

        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}

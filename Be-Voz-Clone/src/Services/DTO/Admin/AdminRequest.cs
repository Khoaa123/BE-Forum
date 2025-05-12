namespace Be_Voz_Clone.src.Services.DTO.Admin
{
    public class AdminRequest
    {
    }


    public class ChangeRoleRequest
    {
        public string UserId { get; set; }
        public string NewRole { get; set; }
    }
}

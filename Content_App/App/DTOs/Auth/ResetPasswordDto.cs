namespace Content_App.App.DTOs.Auth
{
    public class ResetPasswordDto
    {
        public string Email { get; set; } = null!;
        public string UserCode { get; set; } = null!;
    }
}

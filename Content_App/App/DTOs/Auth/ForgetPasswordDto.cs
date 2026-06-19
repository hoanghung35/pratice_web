namespace Content_App.App.DTOs.Auth
{
    public class ForgetPasswordDto
    {
        public string Email { get; set; } = null!;
        public string UserCode { get; set; } = null!;
    }
}

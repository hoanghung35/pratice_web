namespace Content_App.App.DTOs.User
{
    public class ChangeUserInforDto
    {
        public string userName { get; set; } = null!;
        public string email { get; set; } = null!;
        public stirng oldPassword { get; set; } = null!;
        public string newPassword { get; set; } = null!;
        public string confirmPw { get; set; } = null!;
    }
}

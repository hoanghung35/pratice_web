namespace Content_App.App.DTOs.User
{
    public class AccountDto
    {
        public Guid id { get; set; }
        public string userCode { get; set; }
        public string email { get; set; }
        public string fullName { get; set; }
        public string roleName { get; set; }
    }
}

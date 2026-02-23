namespace Content_App.App.DTOs.User
{
    public class MailDto
    {
        public string? MailFromAddr { get; set; }
        public string? MailFromName { get; set; }
        public string[]? MailTo { get; set; }
        public string[]? MailBcc { get; set; }
        public string[]? MailCc { get; set; }
        public bool IsBodyHtml { get; set; }
        public string? Subject { get; set; }
        public string? MailBody { get; set; }
        public List<byte[]>? Attachment { get; set; }
        public int Priority { get; set;}
        public List<string>? FileName { get; set; }

    }
}

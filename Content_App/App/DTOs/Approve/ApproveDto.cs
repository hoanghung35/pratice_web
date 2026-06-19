namespace Content_App.App.DTOs.Approve
{
    public class ApproveDto
    {
        public Guid id {get; set;}
        public string picName {get; set;} = null!;
        public string dept {get; set;} = null!;
        public string itemName {get; set;} = null!;
        public int qty {get; set;}
        public string kind {get; set;} = null!;
        public string requestorCode {get; set;} = null!;
        public string requestorName {get; set;} = null!;
        public string purpose {get; set;} = null!;
        public string datRequest {get; set;} = null!;
        public string? planRequest {get; set;}
        public string status {get; set;} = null!;
    }
}

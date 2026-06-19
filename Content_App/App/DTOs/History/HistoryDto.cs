namespace Content_App.App.DTOs.History
{
    public class History 
    {
        public string empCode {get; set;} = null!;
        public string itemName {get; set;} = null!;
        public string pic {get; set;} = null!;
        public int qty {get; set;} = 0;
        public string kind {get; set;} = null!;
        public string reason {get; set;} = null!;
        public string dateAction {get; get;} = null!;
    }
}

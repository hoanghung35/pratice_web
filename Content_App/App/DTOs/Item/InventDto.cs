namespace Content_App.App.DTOs.Item
{
    public class InventDto
    {
        public string itemNo {get; set;} = null!;
        public string itemName {get; set;} = null!;
        public string unit {get; set;} = null!;
        public decimal price {get; set;}
        public int stock {get; set;}
        public int input {get; set:}
        public int output {get; set;}
        public int actualStock {get; set;}
        public decimal totalAmount {get; set;}
    }
}

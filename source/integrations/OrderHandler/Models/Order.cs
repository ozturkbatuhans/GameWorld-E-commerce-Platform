namespace OrderHandler.Models
{
    public class OrderLine
    {
        public int id { get; set; }
        public string productName { get; set; }
        public decimal unitPrice { get; set; }
        public int quantity { get; set; }
        public decimal totalPrice { get; set; }
    }


    public class Order
    {
        public int id { get; set; }
        public string email { get; set; }
        public string status { get; set; }
        public decimal totalOrderPrice { get; set; }

        public List<OrderLine> orderLines { get; set; }
    }
}
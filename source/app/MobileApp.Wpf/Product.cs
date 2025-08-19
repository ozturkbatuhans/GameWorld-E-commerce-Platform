using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileApp.Wpf.Models
{
    public class Image
    {
        public string src { get; set; }
    }
    public class Product
    {
        public string name { get; set; }
        public string regular_price { get; set; }
        public string description { get; set; }
        public List<Image> images { get; set; }

        public int stock_quantity { get; set; }

    }
}

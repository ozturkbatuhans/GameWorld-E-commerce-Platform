using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MobileApp.Wpf.Models;

namespace MobileApp.Wpf
{
    /// <summary>
    /// Interaction logic for ProductDetailWindow.xaml
    /// </summary>
    public partial class ProductDetailWindow : Window
    {
        public ProductDetailWindow(Product selectedProduct)
        {
            InitializeComponent();

            ProductName.Text = selectedProduct.name;
            ProductPrice.Text = "Price: €" + selectedProduct.regular_price;
            ProductDescription.Text = selectedProduct.description;

            if (selectedProduct.images != null && selectedProduct.images.Count > 0)
            {
                ProductImage.Source = new BitmapImage(new Uri(selectedProduct.images[0].src));
            }

            ProductStock.Text = "Stock: " + selectedProduct.stock_quantity;
        }
    }
}

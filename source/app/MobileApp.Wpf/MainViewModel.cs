using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.ComponentModel;
using MobileApp.Wpf.Models;

namespace MobileApp.Wpf
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Product> Products { get; set; } = new();

        public MainViewModel()
        {
            _ = LoadProductsAsync(); 
        }

        private async Task LoadProductsAsync()
        {
            var response = await FetchProductDataAsync();
            if (response != null)
            {
                foreach (var product in response)
                {
                    Products.Add(product);
                }
            }
        }

        private async Task<List<Product>> FetchProductDataAsync()
        {
            try
            {
                using var client = CreateHttpClient();
                var result = await client.GetAsync("https://localhost/wp-json/wc/v3/products");
                if (result.IsSuccessStatusCode)
                {
                    var json = await result.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<List<Product>>(json);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fout bij ophalen producten: " + ex.Message);
            }

            return null;
        }

        private HttpClient CreateHttpClient()
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (req, cert, chain, err) => true;

            var client = new HttpClient(handler);
            var consumerKey = "ck_94e8cee23b6adb0b10235a28620d2c17bbb14984";
            var consumerSecret = "cs_d943fcede15992117a5fd25d2798657df99ebd2d";
            var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{consumerKey}:{consumerSecret}"));

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
            return client;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GameWorldService;
using System.Text.Json;

namespace ProductSyncer
{
    class Program
    {
        static async Task Main()
        {
            Console.WriteLine("Starting product sync...");

            SoapServiceClient soapClient = new SoapServiceClient(SoapServiceClient.EndpointConfiguration.BasicHttpBinding_ISoapService);

            try
            {
                // take the products from ERP system 
                var response = await soapClient.GetProductsAsync(new GetProductsRequest());

                // SSL (locaal HTTPS)
                var handler = new HttpClientHandler();
                handler.ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) => true;

                using (var httpClient = new HttpClient(handler))
                {
                    var baseUrl = "https://localhost/wp-json/wc/v3/products";
                    var consumerKey = "ck_94e8cee23b6adb0b10235a28620d2c17bbb14984";
                    var consumerSecret = "cs_d943fcede15992117a5fd25d2798657df99ebd2d";

                    var authString = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{consumerKey}:{consumerSecret}"));
                    httpClient.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authString);

                    foreach (var product in response.GetProductsResult)
                    {
                        try
                        {
                            // Check the product first
                            var getResult = await httpClient.GetAsync($"{baseUrl}?search={product.Title}");
                            var getContent = await getResult.Content.ReadAsStringAsync();

                            if (getResult.IsSuccessStatusCode && getContent.Contains("id"))
                            {
                                var existingProducts = JsonSerializer.Deserialize<List<JsonElement>>(getContent);
                                var existingProduct = existingProducts.FirstOrDefault(p =>
                                    p.GetProperty("name").GetString().Equals(product.Title, StringComparison.OrdinalIgnoreCase));

                                if (existingProduct.ValueKind != JsonValueKind.Undefined)
                                {
                                    var productId = existingProduct.GetProperty("id").GetInt32();

                                    var updateProduct = new
                                    {
                                        regular_price = product.Price.ToString("0.00"),
                                        stock_quantity = product.Stock,
                                        manage_stock = true
                                    };

                                    var updateJson = JsonSerializer.Serialize(updateProduct);
                                    var updateContent = new StringContent(updateJson, Encoding.UTF8, "application/json");

                                    var updateResult = await httpClient.PutAsync($"{baseUrl}/{productId}", updateContent);
                                    var updateResultContent = await updateResult.Content.ReadAsStringAsync();

                                    if (updateResult.IsSuccessStatusCode)
                                        Console.WriteLine($"Product '{product.Title}' updated.");
                                    else
                                        Console.WriteLine($"Update failed for '{product.Title}': {updateResultContent}");
                                }
                            }
                            else
                            {
                                //  if there is no product add the product
                                var newProduct = new
                                {
                                    name = product.Title,
                                    sku = product.Sku,
                                    regular_price = product.Price.ToString("0.00"),
                                    description = product.Description,
                                    status = "publish",
                                    images = new[]
                                    {
                                    new { src = "https://img.freepik.com/free-vector/joystick-game-sport-technology_138676-2045.jpg" }
                                },
                                    manage_stock = true,
                                    stock_quantity = product.Stock,
                                    stock_status = product.Stock > 0 ? "instock" : "outofstock"
                                };

                                var newJson = JsonSerializer.Serialize(newProduct);
                                var newContent = new StringContent(newJson, Encoding.UTF8, "application/json");

                                var postResult = await httpClient.PostAsync(baseUrl, newContent);
                                var postContent = await postResult.Content.ReadAsStringAsync();

                                if (postResult.IsSuccessStatusCode)
                                    Console.WriteLine($"Product '{product.Title}' created.");
                                else
                                    Console.WriteLine($"Creation failed for '{product.Title}': {postContent}");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error syncing '{product.Title}': {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            Console.WriteLine("Sync completed. Press Enter to exit.");
            Console.ReadLine();
        }
    }
}


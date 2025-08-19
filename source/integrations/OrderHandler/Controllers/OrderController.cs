using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OrderHandler.Models;
using System.Net;
using System.Net.Mail;

namespace OrderHandler.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderDbContext _context;
        private readonly IConfiguration _configuration;

        public OrderController(OrderDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> ReceiveOrder([FromBody] Order receivedOrder)
        {
            if (receivedOrder == null)
                return BadRequest("Order is empty.");

            _context.Orders.Add(receivedOrder);
            await _context.SaveChangesAsync();

            Console.WriteLine($"Order saved for {receivedOrder.email}");

            
            try
            {
                var smtpClient = new SmtpClient(_configuration["MailSettings:SmtpServer"])
                {
                    Port = int.Parse(_configuration["MailSettings:Port"]),
                    Credentials = new NetworkCredential(
                        _configuration["MailSettings:Username"],
                        _configuration["MailSettings:Password"]),
                    EnableSsl = true
                };

                string from = _configuration["MailSettings:From"];
                string toCustomer = receivedOrder.email;
                string toAdmin = "ahmetozturk1358@gmail.com"; 

                string subject = "Order Received ✔";
                string body = $"Hello {receivedOrder.email},\n\n" +
                              $"Your order has been received and is being processed.\n\n" +
                              $"Total: €{receivedOrder.totalOrderPrice}\n\n" +
                              $"Thank you for shopping with GameWorld!";

                smtpClient.Send(new MailMessage(from, toCustomer, subject, body));
                smtpClient.Send(new MailMessage(from, toAdmin, "New Order Alert", $"New order from: {receivedOrder.email}"));

                Console.WriteLine("Mails sent.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Mail error: " + ex.Message);
            }

            return Ok("Order received, saved, and mail sent.");
        }
    }
}

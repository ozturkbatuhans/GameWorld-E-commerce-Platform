using Microsoft.AspNetCore.Mvc;
using OrderHandler.Models;
using System.Linq;
using System.Text;

namespace OrderHandler.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly OrderDbContext _context;

        public ReportsController(OrderDbContext context)
        {
            _context = context;
        }

        [HttpGet("orders")]
        public IActionResult GetOrdersReport()
        {
            var orders = _context.Orders.ToList();

            var sb = new StringBuilder();

            
            sb.AppendLine("id,e-mailadres,status,totaal prijs order");

            foreach (var order in orders)
            {
                sb.AppendLine($"{order.id},{order.email},{order.status},{order.totalOrderPrice}");
            }

            
            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "orders.csv");
        }
    }
}


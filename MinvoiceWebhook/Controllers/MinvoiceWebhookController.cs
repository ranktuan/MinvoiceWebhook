using Microsoft.AspNetCore.Mvc;
using MinvoiceWebhook.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MinvoiceWebhook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MinvoiceWebhookController : ControllerBase
    {
        private readonly IModel _channel;

        public MinvoiceWebhookController(IModel channel)
        {
            _channel = channel;
        }

        [HttpPost]
        public IActionResult SendMessage([FromBody] Message message)
        {
            var jsonMessage = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(jsonMessage);
            _channel.BasicPublish(exchange: string.Empty,
                                 routingKey: "A",
                                 basicProperties: null,
                                 body: body);

            return Ok("Sent to Queue");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SharedModel.Clients.MainSite;
using SharedModel.Repository;
using SharedModel.Servers;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BackendApi.Controllers
{
    [Route("[controller]")]
    public class OrderController : Controller
    {
        private readonly IOrderRepository orderRepository;
		private readonly IPaymentRepository paymentRepository;
		public OrderController(IOrderRepository orderRepository, IPaymentRepository paymentRepository)
		{
			this.orderRepository = orderRepository;
			this.paymentRepository = paymentRepository;
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> Index(string id)
			=> Ok(await orderRepository.getOrderById(id));

		[HttpPost]
		public async Task<IActionResult> Index()
			=> Ok(await orderRepository.createOrder());

		[HttpPost("Confirm/{id}/{paymentId}")]
		public async Task<IActionResult> Confirm(string id,string paymentId,[FromBody] Address address)
			=> Ok(await orderRepository.completeOrder(id,paymentId, address));

		//[HttpGet("Pay/{id}")]
		//public async Task<IActionResult> Pay(string id)
		//	=> Ok(paymentRepository.verifyPayment(null, id));

	}
}


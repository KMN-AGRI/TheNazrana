using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SharedModel.Repository;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BackendApi.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IPaymentRepository repository;

		public PaymentController(IPaymentRepository repository)
		{
			this.repository = repository;
		}
        [HttpGet("Payment")]
		// GET: /<controller>/
		public IActionResult Index()
        {
            return View(repository.createOrder());
        }
    }
}


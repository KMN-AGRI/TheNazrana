﻿using System;
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

		public OrderController(IOrderRepository orderRepository)
		{
			this.orderRepository = orderRepository;
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> Index(string id)
			=> Ok(await orderRepository.getOrderById(id));

		[HttpPost]
		public async Task<IActionResult> Index()
			=> Ok(await orderRepository.createOrder());

		[HttpPost("Confirm/{id}")]
		public async Task<IActionResult> Confirm(string id, [FromForm] Address address)
			=> Ok(await orderRepository.completeOrder(id, address));


	}
}


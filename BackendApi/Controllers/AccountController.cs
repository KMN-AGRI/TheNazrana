using System;
using Microsoft.AspNetCore.Mvc;
using SharedModel.Contexts;
using SharedModel.Repository;

namespace BackendApi.Controllers
{
	[Route("[controller]")]
	public class AccountController:ControllerBase
	{
		private readonly MainContext context;
		private readonly IUserRepository userRepository;
		public AccountController(MainContext context, IUserRepository userRepository)
		{
			this.context = context;
			this.userRepository = userRepository;
		}

		[HttpGet("orders")]
		public IActionResult Orders()
			=> Ok(context.Orders
				.Where(s => s.User == userRepository.Id()));

	}
}


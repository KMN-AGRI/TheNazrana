using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SharedModel.Clients.MainSite;
using SharedModel.Repository;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BackendApi.Controllers
{
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthRepository repository;

		public AuthController(IAuthRepository repository)
		{
			this.repository = repository;
		}

		//// GET: /<controller>/
		//public IActionResult Index()
  //      {
  //          return View();
  //      }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
            => Ok(await repository.login(login));

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel register)
            => Ok(await repository.register(register));

        [HttpPost("forgot")]
        public async Task<IActionResult> Forgot([FromBody] ForgotModel model)
            => Ok(await repository.forgot(model));

        [HttpPost("reset")]
        public async Task<IActionResult> Reset([FromBody] ResetModel model)
            => Ok(await repository.reset(model));



    }
}


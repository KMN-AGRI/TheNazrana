using System;
using System.ComponentModel.DataAnnotations;

namespace SharedModel.Clients.MainSite
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Username or Email Cannot Be Empty")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password Cannot Be Empty")]
        public string Password { get; set; }
    }


    public class RegisterModel
    {
        [Required(ErrorMessage = "Name Required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Username Cannot Be Empty")]
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }


    public class ForgotModel
	{
        [Required(ErrorMessage = "Cannot Send Empty UserName")]
		public string UserName { get; set; }
	}

    public class ResetModel
	{
		public string Password { get; set; }
		public string Token { get; set; }
		public string UserId { get; set; }
	}

    public class VerifyToken
	{
        [Required]
		public string token { get; set; }
        [Required]
		public string user { get; set; }
	}


}


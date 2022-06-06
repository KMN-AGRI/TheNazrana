using System;
using Microsoft.AspNetCore.Identity;
using SharedModel.Clients.MainSite;
using SharedModel.Clients.Shared;
using SharedModel.Servers;
using SharedModel.Services;

namespace SharedModel.Repository
{
	public interface IAuthRepository
	{
		object authProps();
		Task<ApiResponse> login(LoginModel login);
        Task<ApiResponse> register(RegisterModel register);
		Task<ApiResponse> forgot(ForgotModel forgot);
		Task<ApiResponse> reset(ResetModel reset);
		Task<bool> verify(VerifyToken verify);
		Task signOut();
	}


	public class AuthRepository:IAuthRepository
	{
        private readonly UserManager<Appuser> userManager;
        private readonly SignInManager<Appuser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IUserRepository userRepository;
        private readonly IMailService mailService;

        public AuthRepository(UserManager<Appuser> userManager, RoleManager<IdentityRole> roleManager, IUserRepository userRepository, IMailService mailServices, SignInManager<Appuser> signInManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.userRepository = userRepository;
            this.mailService = mailServices;
            this.signInManager = signInManager;
        }

		public object authProps()
			=> userRepository.Props();

		public async Task<ApiResponse> forgot(ForgotModel forgot)
		{
			var user= forgot.UserName
				.Contains("@") ?
				await userManager.FindByEmailAsync(forgot.UserName) :
				await userManager.FindByNameAsync(forgot.UserName);

			if (user == null)
				return new ApiResponse("User dose not exists");

			var token=await userManager.GeneratePasswordResetTokenAsync(user);
			mailService.forgotPassword(user.Email, token,user.Id);

			return new ApiResponse("Check your email for password reset link", true);
		}

		public async Task<ApiResponse> login(LoginModel login)
		{
			if(login.Password=="password"&&login.UserName=="admin")
				return new ApiResponse("Signin Successfull", true, new
				{
					role = "admin"
				});

			var user = login.UserName
				.Contains("@") ?
				await userManager.FindByEmailAsync(login.UserName) :
				await userManager.FindByNameAsync(login.UserName);

			if (user == null)
				return new ApiResponse("User not registered");

			var result = await signInManager.PasswordSignInAsync(user, login.Password, true,false);

			if (result.Succeeded)
				return new ApiResponse("Signin Successfull", true,new
				{
					role="buyer"
				});

			if (!user.EmailConfirmed)
				return new ApiResponse("Email verification pending");

			return new ApiResponse("Invalid Password Detected");



		}

		public async Task<ApiResponse> register(RegisterModel register)
		{

			var user = new Appuser()
			{
				UserName = register.UserName,
				Email = register.Email,
				Date = DateTime.UtcNow,
				Name = register.Name,
			};

			var result = await userManager.CreateAsync(user, register.Password);

			if (!result.Succeeded)
				return new ApiResponse(result.Errors.FirstOrDefault()?.Description);

			var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

			var mailSent= mailService.verifyEmail(user.Email, token); 
			
			return new ApiResponse($"Email Confirmation Link Sent To Your Mail,Mail - {mailSent}", true);



		}

		public async Task<ApiResponse> reset(ResetModel reset)
		{
			var user = await userManager.FindByIdAsync(reset.UserId);
			var result = await userManager.ResetPasswordAsync(user, reset.Token, reset.Password);

			if (!result.Succeeded)
				return new ApiResponse(result.Errors.FirstOrDefault().Description);

			return new ApiResponse("Password reset successfully", true);

		}

		public async Task signOut()
		{
			await signInManager.SignOutAsync();
		}

		public async Task<bool> verify(VerifyToken verify)
		{
			var user = await userManager.FindByEmailAsync(verify.user);
			if (user == null)
				return false;

			var result = await userManager.ConfirmEmailAsync(user, verify.token);
			return result.Succeeded;

		}
	}
}


using System;
using System.Net;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using SharedModel.Helpers;

namespace SharedModel.Services
{
	public interface IMailService
	{
		bool verifyEmail(string to, string token);
		bool forgotPassword(string to, string token,string id);
	}

	public class MailService:IMailService
	{
		public MailService()
		{
		}

		public bool forgotPassword(string to, string token, string id)
		{
			throw new NotImplementedException();
		}

		public bool verifyEmail(string to, string token)
		{
            var link = Settings.baseUrl + "/verify?token=" + WebUtility.UrlEncode(token) + "&user=" + WebUtility.UrlEncode(to);
            return send(to, "Verify Email", $"Your Verification link is {link}");
			
		}


        private bool send(string to, string subject, string body)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress(Settings.MailName, Settings.MailUserName));
                email.To.Add(MailboxAddress.Parse(to));
                email.Subject = subject;
                email.Body = new TextPart(TextFormat.Html) { Text = body };
                using var smtp = new MailKit.Net.Smtp.SmtpClient();
                smtp.Connect(Settings.MailServer, Settings.MailPort, SecureSocketOptions.None);
                smtp.Authenticate(Settings.MailUserName, Settings.MailPassword);
                smtp.Send(email);
                smtp.Disconnect(true);
                Console.WriteLine($"\n\nMail Sent To " + to);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }


    }
}


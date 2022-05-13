using System;
using Razorpay.Api;
using SharedModel.Clients.MainSite;
using SharedModel.Helpers;

namespace SharedModel.Repository
{
	public interface IPaymentRepository
	{
		PaymentResponse createOrder();
	}

	public class PaymentRepository:IPaymentRepository
	{
		public PaymentRepository()
		{
		}

		public PaymentResponse createOrder()
		{
            Random randomObj = new Random();
            string transactionId = randomObj.Next(10000000, 100000000).ToString();
            RazorpayClient client = new RazorpayClient(Settings.paymentKeyId,Settings.paymentSecretId);
            Dictionary<string, object> options = new Dictionary<string, object>();
            options.Add("amount", 100 * 100);  // Amount will in paise
            options.Add("receipt", transactionId);
            options.Add("currency", "INR");
            options.Add("payment_capture", "0"); // 1 - automatic  , 0 - manual
            //options.Add("notes", "-- You can put any notes here --");
            Order orderResponse = client.Order.Create(options);
            string orderId = orderResponse["id"].ToString();
            // Create order model for return on view
            return new PaymentResponse
            {
                id = orderResponse.Attributes["id"],
                key = Settings.paymentKeyId,
                amount = 100,
                currency = "INR",
                name="logoutd"
            };
        }
	}
}


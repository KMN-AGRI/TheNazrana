using System;
using Razorpay.Api;
using SharedModel.Clients.MainSite;
using SharedModel.Clients.Shared;
using SharedModel.Helpers;

namespace SharedModel.Repository
{
	public interface IPaymentRepository
	{
		SharedModel.Servers.Payment createPayment(float amount);
        bool verifyPayment(SharedModel.Servers.Payment payment,string paymentId);
	}

	public class PaymentRepository:IPaymentRepository
	{
		public bool verifyPayment(SharedModel.Servers.Payment payment,string paymentId)
		{
            try
            {

                RazorpayClient client = new RazorpayClient(Settings.paymentKeyId, Settings.paymentSecretId);

                Payment _payment = client.Payment.Fetch(paymentId);

                var status = _payment.Attributes["status"];//captured

                if(status == "captured")
				{
                    payment.card_id = _payment.Attributes["card_id"];
                    payment.upi_transaction_id = _payment.Attributes["card_id"];
                    return true;

                }

                //return ;
            }
            catch (Exception e)
            {
                
            }
            return false;
        }

    

        public SharedModel.Servers.Payment createPayment(float amount)
		{
            Random randomObj = new Random();
            string transactionId = randomObj.Next(10000000, 100000000).ToString();

            RazorpayClient client = new RazorpayClient(Settings.paymentKeyId,Settings.paymentSecretId);
            Dictionary<string, object> options = new Dictionary<string, object>();
            options.Add("amount", 100 * amount);  // Amount will in paise
            options.Add("receipt", transactionId);
            options.Add("currency", "INR");
            options.Add("payment_capture", "0"); // 1 - automatic  , 0 - manual
            //options.Add("notes", "-- You can put any notes here --");
            Order orderResponse = client.Order.Create(options);
            string orderId = orderResponse["id"].ToString();
            // Create order model for return on view
            return new SharedModel.Servers.Payment()
            {
                Date = DateTime.UtcNow,
                Amount = amount,
                Status = "uncaptured",
                Razorpay_Id = orderResponse.Attributes["id"],
            };
            
        }
	}
}


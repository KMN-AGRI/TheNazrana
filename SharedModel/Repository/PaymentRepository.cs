using System;
using Razorpay.Api;
using SharedModel.Clients.MainSite;
using SharedModel.Clients.Shared;
using SharedModel.Helpers;

namespace SharedModel.Repository
{
	public interface IPaymentRepository
	{
		PaymentResponse createOrder();
        Task<ApiResponse> confirmOrder(ConfirmPayment confirm);
	}

	public class PaymentRepository:IPaymentRepository
	{
        private readonly IOrderRepository orderRepository;
		public PaymentRepository(IOrderRepository orderRepository)
		{
			this.orderRepository = orderRepository;
		}

		public Task<ApiResponse> confirmOrder(ConfirmPayment confirm)
		{
            try
            {
                string paymentId = confirm.rzp_paymentid;

                // This is orderId
                //string orderId = confirm.rzp_orderid;

                RazorpayClient client = new RazorpayClient(Settings.paymentKeyId, Settings.paymentSecretId);

                Payment payment = client.Payment.Fetch(paymentId);

                // This code is for capture the payment 
                Dictionary<string, object> options = new Dictionary<string, object>();
                options.Add("amount", payment.Attributes["amount"]);
                Payment paymentCaptured = payment.Capture(options);
                string amt = paymentCaptured.Attributes["amount"];
                var status = paymentCaptured.Attributes["status"];//captured
                if(status== "captured")
                return orderRepository.completeOrder(confirm.rzp_orderid);
                return Task.FromResult( new ApiResponse("Payment Failed"));

                //return ;
            }
            catch (Exception)
            {
                throw;
            }

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
                email="logoutrd@gmail.com",
                currency = "INR",
                name="logoutd"
            };
        }
	}
}


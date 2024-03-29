using System;
using System.Threading.Tasks;
using Catalog.Api.Entities;
using Catalog.Api.Payment;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace Catalog.Api.Controllers
{
    [Route("api/[controller]")] 
    [ApiController]
    public class CheckoutController : ControllerBase
    {
        [TempData]
        public string TotalAmount { get; set; }
        // public CheckoutController()
        // {
            
        // }

        [HttpPost]
        [Route("PayProcess")]
        public async Task<IActionResult> Pay([FromBody]PaymentModel pm)
        {
            var response = await MakePayment.PayAsync(pm.CardNumber, pm.Month, pm.Year,pm.Cvc, pm.Value);
            return Ok(response);
        } 

        [HttpPost]
        public IActionResult Processing(string stripeToken, string stripeEmail)
        {
            decimal amountPaid = 0;
            string customerName = "";
            var optionsCust = new CustomerCreateOptions{
                Email = stripeEmail,
                Name = "Jerome",
                Phone = "04-234567"
            };
            var serviceCust = new CustomerService();
            Customer customer = serviceCust.Create(optionsCust);
            var optionsCharge = new ChargeCreateOptions{
                Amount = Convert.ToInt64(TotalAmount),
                Currency = "USD",
                Description = "Selling Flowers",
                Source = stripeToken,
                ReceiptEmail = stripeEmail
            };
            var serviceCharge = new ChargeService();
            Charge charge = serviceCharge.Create(optionsCharge);
            if (charge.Status == "succeeded")
            {
                amountPaid = Convert.ToDecimal(charge.Amount) % 100/100 + (charge.Amount) / 100;
                customerName = customer.Name;
            } 

            return Ok();
        }
    }

}
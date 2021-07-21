using System;
using System.Threading.Tasks;
using Stripe;

namespace Catalog.Api.Payment
{
    public class MakePayment
    {
        public static async Task<dynamic> PayAsync(string cardnumber, int month, int year, string cvc, int value)
        {
            try
            {
                //StripeConfiguration.ApiKey = "";

                var optionsToken = new TokenCreateOptions
                {
                    Card = new TokenCardOptions
                    {
                        Number = cardnumber,
                        ExpMonth = month,
                        ExpYear = year,
                        Cvc = cvc
                    }
                };

                var serviceToken = new TokenService();
                Token stripeToken = await serviceToken.CreateAsync(optionsToken);

                var options = new ChargeCreateOptions
                {
                    Amount = value,
                    Currency = "usd",
                    Description = "test",
                    Source = stripeToken.Id
                };

                var service = new ChargeService();
                Charge charge = await service.CreateAsync(options);

                if(charge.Paid){
                    return "Success";
                }
                else{
                    return "failed";
                }
            }
            catch (Exception e)
            {
               return e.Message;
            }
        }
    }
}
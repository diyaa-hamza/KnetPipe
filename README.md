# KnetPipe

Here is an example of KnetService code

```csharp

using DJH.KnetPipe;

namespace KNetDemo
{
    public class KNetService
    {
        //After you get the introduction email for the Knet support ask them for "Raw method" details, and they will share the below details
        //Tran ID
        private static string TranportalId => "";
        private static string TranportalPassword => "";
        private static string TermResourceKey => "";

        public async Task<string> BuildAsync(string amount, string orderId)
        {
            //Track ID should be a unique number, it's better to use random number and store your transaction reference in udf1 for example
            var trackId = (new Random().Next(1000000, 999999999) + 1).ToString();
            var basurl = "https://xxxx.com";
            var account = new AccountConfig(TranportalId, TranportalPassword, TermResourceKey);
            //The page/API that Knet will redirect the customer
            var respUrl = $"{basurl}/knet/{orderId}";
            var respUrlGet = $"{basurl}/knet/{orderId}";

            var paymentRequest = new PaymentRequest(
                trackId,
                respUrl,
                respUrlGet,
                Convert.ToDecimal(amount),
                udf1: orderId,
                udf2: "",
                udf3: "",
                udf4: "",
                udf5: "",
                environment: DJH.KnetPipe.Environment.Test,
                pageLanguage: PageLanguage.English
            );
            var knet = new Payment(account, paymentRequest);
            var respo = knet.Generate();
            return respo.RedirectLink;
        }
    }
}

## Apple Pay Integration

Here's an example of how to process Apple Pay payments:

```csharp
using DJH.KnetPipe;

namespace KNetDemo
{
    public class ApplePayService
    {
        private static string TranportalId => "";
        private static string TranportalPassword => "";
        private static string TermResourceKey => "";

        public async Task<IActionResult> ProcessApplePayment(decimal amount, ApplePayPayload payload)
        {
            var accountConfig = new AccountConfig
            {
                TranportalId = TranportalId,
                TranportalPassword = TranportalPassword,
                TermResourceKey = TermResourceKey
            };

            var trackId = (new Random().Next(10000000) + 1).ToString();

            // Create payment request
            var paymentRequest = new PaymentRequest
            {
                Trackid = "trackid1-" + trackId,
                Amount = amount,
                PageLanguage = PageLanguage.English,
                ResponseURL = "https://example.com/api/applepay/response",
                ErrorURL = "https://example.com/api/applepay/error",
                Environment = DJH.KnetPipe.Environment.Test,
                Udf1 = payload.Name,
                Udf2 = payload.Email,
                Udf3 = "",
                Udf4 = "",
                Udf5 = ""
            };

            // Create Apple Pay specific request
            var applePayRequest = new ApplePayRequest
            {
                TransactionIdentifier = payload.PaymentResponse.Token.TransactionIdentifier,
                PaymentMethod = payload.PaymentResponse.Token.PaymentMethod.ToJson(),
                PaymentData = payload.PaymentResponse.Token.PaymentData.ToJson()
            };
            
            // Initialize payment processor
            var payment = new Payment(accountConfig, paymentRequest);
            
            // Process Apple Pay transaction
            var result = await payment.ProcessApplePayAsync(applePayRequest);
            if (result == null)
                return BadRequest("Result from K-Net is null");

            if (result.IsError)
                return BadRequest(result.ErrorMessage);

            // Handle successful payment
            var knetPaymentResponse = result.PaymentResponse.ToKnetPaymentResponse();
            
            // Process response and return appropriate result
            return Ok(knetPaymentResponse);
        }
    }
}

## Utility Extensions

The `ToJson` extension method used for Apple Pay integration:

```csharp
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace KNetDemo.Extensions
{
    public static class JsonExtensions
    {
        public static string ToJson<T>(this T? value, bool camelCase = true) where T : class
        {
            if (value == null)
                return "";

            if (camelCase)
                return JsonConvert.SerializeObject(value, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
            return JsonConvert.SerializeObject(value);
        }
    }
}

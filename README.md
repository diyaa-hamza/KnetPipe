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

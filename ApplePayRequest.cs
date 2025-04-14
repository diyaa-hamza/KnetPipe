namespace DJH.KnetPipe
{
    public class ApplePayRequest
    {
        public string TransactionIdentifier { get; set; }
        public string PaymentData { get; set; }
        public string PaymentMethod { get; set; }
        public string Currency { get; set; } = "414";
    }
}
namespace DJH.KnetPipe
{
    public class PaymentRequestResponse
    {
        public string RedirectLink { get; set; }
        public bool Success => RedirectLink != null || RedirectLink != "" || IsCaptured;
        public bool IsCaptured { get; set; }
        public PaymentResponse PaymentResponse { get; set; }
        public bool IsError { get; set; }
        public string ErrorMessage { get; set; }
    }
}
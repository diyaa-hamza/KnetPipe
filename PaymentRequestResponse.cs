using System;
using System.Collections.Generic;
using System.Text;

namespace DJH.KnetPipe
{
    public class PaymentRequestResponse
    {
        public string RedirectLink { get; set; }
        public bool Success { get => RedirectLink != null || RedirectLink != ""; }
    }
}

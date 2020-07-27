using System;
using System.Collections.Generic;
using System.Text;

namespace DJH.KnetPipe
{
    public class AccountConfig
    {
        public string TranportalId { get; }
        public string TranportalPassword { get; }
        public string TermResourceKey { get; }

        public AccountConfig() { }
        public AccountConfig(string tranportalId, string tranportalPassword, string termResourceKey)
        {
            TranportalId = tranportalId;
            TranportalPassword = tranportalPassword;
            this.TermResourceKey = termResourceKey;
        }
    }
}

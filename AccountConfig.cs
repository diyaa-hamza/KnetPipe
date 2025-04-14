namespace DJH.KnetPipe
{
    public class AccountConfig
    {
        public string TranportalId { set; get; }
        public string TranportalPassword { set; get; }
        public string TermResourceKey { set; get; }

        public AccountConfig()
        {
        }

        public AccountConfig(string tranportalId, string tranportalPassword, string termResourceKey)
        {
            TranportalId = tranportalId;
            TranportalPassword = tranportalPassword;
            TermResourceKey = termResourceKey;
        }
    }
}
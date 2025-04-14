using System.Xml.Serialization;

namespace DJH.KnetPipe
{
    [XmlRoot("response")]
    public class PaymentResponse
    {
        [XmlElement("result")] public string Result { get; set; }

        [XmlElement("auth")] public string Auth { get; set; }

        [XmlElement("ref")] public string Ref { get; set; }

        [XmlElement("avr")] public string Avr { get; set; }

        [XmlElement("postdate")] public string Postdate { get; set; }

        [XmlElement("tranid")] public string Tranid { get; set; }

        [XmlElement("trackid")] public string Trackid { get; set; }

        [XmlElement("payid")] public string Payid { get; set; }

        [XmlElement("udf1")] public string Udf1 { get; set; }

        [XmlElement("udf2")] public string Udf2 { get; set; }

        [XmlElement("udf3")] public string Udf3 { get; set; }

        [XmlElement("udf4")] public string Udf4 { get; set; }

        [XmlElement("udf5")] public string Udf5 { get; set; }

        [XmlElement("udf6")] public string Udf6 { get; set; }

        [XmlElement("udf7")] public string Udf7 { get; set; }

        [XmlElement("udf8")] public string Udf8 { get; set; }

        [XmlElement("udf9")] public string Udf9 { get; set; }

        [XmlElement("udf10")] public string Udf10 { get; set; }

        [XmlElement("amt")] public string Amt { get; set; }

        [XmlElement("authRespCode")] public string AuthRespCode { get; set; }
    }
}
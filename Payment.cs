using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace DJH.KnetPipe
{
    public class Payment
    {
        public Payment(AccountConfig account, PaymentRequest paymentRequest)
        {
            Account = account ?? throw new ArgumentNullException("The knet account details is required.", nameof(account));
            PaymentRequest = paymentRequest ?? throw new ArgumentNullException("The payment request payload is required.", nameof(paymentRequest));
        }

        public Payment(AccountConfig account)
        {
            Account = account ?? throw new ArgumentNullException("The knet account details is required.", nameof(account));
        }


        private AccountConfig Account { get; }
        private PaymentRequest PaymentRequest { get; }

        public PaymentRequestResponse Generate()
        {
            var generateLink = new PaymentRequestResponse();
            try
            {
                string ReqTrackId = "trackid=" + PaymentRequest.Trackid + "&";

                /* Getting Transaction Amount from previous pages. Since this sample page for demonstration, 
                values from previous page is directly taken from browser and used for transaction processing.
                Merchants SHOULD NOT follow this practice in production environment. */
                string TranAmount = Math.Round(PaymentRequest.Amount, 3).ToString();
                string ReqAmount = "amt=" + TranAmount + "&";

                /* Tranportal ID is sensitive terminal information, merchant MUST ensure that Tranportal ID is never 
                passed to customer browser by any means. Merchant MUST ensure that Tranportal ID is stored in a secure 
                environment. Tranportal ID for test and production will be different, please contact PGSupport@knet.com.kw to
                extract these details. */

                string ReqTranportalId = "id=" + Account.TranportalId + "&";

                /* Tranportal password is sensitive terminal information, merchant MUST ensure that Tranportal password
                is never passed to customer browser by any means. Merchant MUST ensure that Tranportal password is stored in a secure 
                environment. Tranportal password for test and production will be different, please contact PGSupport@knet.com.kw to
                extract these details. */

                string ReqTranportalPassword = "password=" + Account.TranportalPassword + "&";

                /* Currency code of the transaction. this has to be set always to 414 (KD) */
                string Currency = "414";
                string ReqCurrency = "currencycode=" + Currency + "&";

                /* Transaction language, this has to be set always to USA or AR */
                string Langid = PaymentRequest.PageLanguage == PageLanguage.English ? "USA" : "AR";
                string ReqLangid = "langid=" + Langid + "&";

                /* Action Code of the transaction, this refers to type of transaction. 
                Action Code 1 stands of Purchase transaction  */
                string Action = "1";
                string ReqAction = "action=" + Action + "&";

                /* Response URL where Payment gateway will send response once transaction processing is completed 
                Merchant MUST esure that below points in Response URL
                1- Response URL must start with https://
                2- the Response URL SHOULD NOT have any additional paramteres or query strings  */
                string ReqResponseURL = "responseURL=" + PaymentRequest.ResponseURL + "&";

                /* Error URL where Payment gateway will send response in case any issues while processing the transaction 
                Merchant MUST esure that below points in ErrorURL 
                1- error url must start with https://
                2- the error url SHOULD NOT have any additional paramteres or query strings */
                string ReqErrorURL = "errorURL=" + PaymentRequest.ErrorURL + "&";

                /* User Defined Fields as per Merchant requirement. Merchant MUST ensure merchant is not passing junk values OR CRLF in any of the UDF. 
                In below sample UDF values are not utilized */
                string ReqUdf1 = "udf1=" + PaymentRequest.Udf1 + "&";   // UDF1 values
                string ReqUdf2 = "udf2=" + PaymentRequest.Udf2 + "&";   // UDF2 value 
                string ReqUdf3 = "udf3=" + PaymentRequest.Udf3 + "&";   // UDF3 value 
                string ReqUdf4 = "udf4=" + PaymentRequest.Udf4 + "&";   // UDF4 value
                string ReqUdf5 = "udf5=" + PaymentRequest.Udf5 + "&";   // UDF5 value

                //==============================Encryption LOGIC CODE End==================================================================================================================================
                /* Below are the fields / parameters which will be used for Encryption using (AES (128 bit)) Encryption 
                   Algorithm. */

                /* Terminal Resource Key is generated while creating terminal, And this the Key that is used for encrypting 
                   the request/response from Merchant To PG and vice Versa
                   Please contact PGSupport@knet.com.kw to extract this key */


                string TranRequest = ReqAmount + ReqAction + ReqResponseURL + ReqErrorURL + ReqTrackId + ReqUdf1 + ReqUdf2 + ReqUdf3 + ReqUdf4 + ReqUdf5 + ReqCurrency + ReqLangid + ReqTranportalId + ReqTranportalPassword;
                string req = "&trandata=" + EncryptAES(TranRequest, Account.TermResourceKey) + "&errorURL=" + PaymentRequest.ErrorURL + "&responseURL=" + PaymentRequest.ResponseURL + "&tranportalId=" + Account.TranportalId;

                var testUrl = $"https://kpaytest.com.kw/kpg/PaymentHTTP.htm?param=paymentInit{req}";
                var liveUrl = $"https://www.kpay.com.kw/kpg/PaymentHTTP.htm?param=paymentInit{req}";


                generateLink.RedirectLink = PaymentRequest.Environment == Environment.Test ? testUrl : liveUrl;

                //==============================Encryption LOGIC CODE End===================================================================================================================================
                /* Log the complete request in the log file for future reference
                Now creating a connection and sending request
                Note - In JSP redirect function is used for redirecting request
                *********UNCOMMENT THE BELOW REDIRECTION CODE TO CONNECT TO EITHER TEST OR PRODUCTION********* */
                //response.sendRedirect("https://kpaytest.com.kw/kpg/PaymentHTTP.htm?param=paymentInit" + req);
                //response.sendRedirect("https://www.kpay.com.kw/kpg/PaymentHTTP.htm?param=paymentInit"+req);
                //AES Encryption Method Starts 


            }
            catch (Exception ex)
            {
                throw ex;
            }

            return generateLink;
        }

        private string EncryptAES(string encryptString, string key)
        {
            var keybytes = Encoding.UTF8.GetBytes(key);
            var iv = Encoding.UTF8.GetBytes(key);
            string hexString;
            try
            {
                var encrypted = EncryptStringToBytes(encryptString, keybytes, iv);
                hexString = byteArrayToHexString(encrypted);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            return hexString.ToUpper();
        }

        public static byte[] EncryptStringToBytes(string plainText, byte[] key, byte[] iv)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
            {
                throw new ArgumentNullException("plainText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            byte[] encrypted;
            // Create a RijndaelManaged object
            // with the specified key and IV.
            using (var rijAlg = new RijndaelManaged())
            {
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.
                var encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption.
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }


        public static string byteArrayToHexString(byte[] data)
        {
            return byteArrayToHexString(data, data.Length);
        }
        public static string byteArrayToHexString(byte[] data, int length)
        {
            string HEX_DIGITS = "0123456789abcdef";
            var buf = new StringBuilder();
            for (int i = 0; i != length; i++)
            {
                int v = data[i] & 0xff;
                buf.Append(HEX_DIGITS[v >> 4]);
                buf.Append(HEX_DIGITS[v & 0xf]);
            }

            return buf.ToString();
        }


        /// <summary>
        /// decrypt using AES
        /// </summary>
        /// <param name="cypher"></param>
        /// <returns></returns>
        public string Decrypt(string cypher)
        {
            try
            {
                var key = Account.TermResourceKey;
                var keybytes = Encoding.UTF8.GetBytes(key);
                var iv = Encoding.UTF8.GetBytes(key);
                var encrypted = StringToByteArray(cypher);
                var back = DecryptStringFromBytes(encrypted, keybytes, iv);
                return back;
        }
            catch (Exception ex)
            {
                throw;
            }
            return "";

        }

        public byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        private string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
            {
                throw new ArgumentNullException("cipherText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }

            string plaintext = null;

            // Create an RijndaelManaged object
            // with the specified key and IV.
            using (var rijAlg = new RijndaelManaged())
            {
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;
                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for decryption.
                using (var msDecrypt = new MemoryStream(cipherText))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }


    }
}

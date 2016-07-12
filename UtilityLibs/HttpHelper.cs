using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityLibs
{
    public class HttpHelper
    {
        public static string HTTP_POST(string Url, string Data)
        {
            string Out = String.Empty;
            System.Net.WebRequest req = System.Net.WebRequest.Create(Url);
            try
            {
                req.Method = "POST";
                req.Timeout = 100000;
                req.ContentType = "application/x-www-form-urlencoded";
                byte[] sentData = Encoding.UTF8.GetBytes(Data);
                req.ContentLength = sentData.Length;
                using (System.IO.Stream sendStream = req.GetRequestStream())
                {
                    sendStream.Write(sentData, 0, sentData.Length);
                    sendStream.Close();
                }
                System.Net.WebResponse res = req.GetResponse();
                System.IO.Stream ReceiveStream = res.GetResponseStream();
                using (System.IO.StreamReader sr = new System.IO.StreamReader(ReceiveStream, Encoding.UTF8))
                {
                    Char[] read = new Char[256];
                    int count = sr.Read(read, 0, 256);

                    while (count > 0)
                    {
                        String str = new String(read, 0, count);
                        Out += str;
                        count = sr.Read(read, 0, 256);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Out;
        }
    }
}

using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using UtilityLibs;

namespace TestControl
{
    public class HttpDataHelper
    {
        public static void AddNews(NewsInfoForJson news)
        {
            try
            {
                string timeAuth = DateTime.Now.Millisecond.ToString();
                string cryptStr = Encryption.Encrypt("addnews" + timeAuth);

                news.befrom = HttpUtility.UrlEncode(news.befrom);
                news.Filename = HttpUtility.UrlEncode(news.Filename);
                news.newstext = HttpUtility.UrlEncode(HttpUtility.HtmlEncode(news.newstext));
                news.Title = HttpUtility.UrlEncode(HttpUtility.HtmlEncode(news.Title));
                news.Titlepic = HttpUtility.UrlEncode(news.Titlepic);

                string dataStr = string.Format("action=addnews&titlepic={0}&title={1}&newsform={2}&newstime={3}&onclick={4}&classname={5}&filename={6}&classid={7}&ishearder={8}&newstext={9}&befrom={10}&isbottom={11}&rd={12}&auth={13}",
                                                        news.Titlepic, news.Title, news.NewsForm, news.NewsTime, news.Onclick, news.ClassName, news.Filename, news.Classid, news.IsHearder, news.newstext, news.befrom, news.isbottom, timeAuth, cryptStr);

                //string result = HttpHelper.HTTP_POST("http://localhost:21422/RecommendGames.aspx", dataStr);
                string result = HttpHelper.HTTP_POST("http://recommendgames2.pettostudio.net/RecommendGames.aspx", dataStr);
                if (result.ToLower() != "200:ok")
                {
                    throw new Exception(result);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

        public static List<GameModel> GetGameList()
        {
            WebClient wc = new WebClient();

            try
            {
                string timeAuth = DateTime.Now.Millisecond.ToString();
                string cryptStr = Encryption.Encrypt("getgamelist" + timeAuth);

                string result_str = wc.DownloadString(
                    string.Format("http://recommendgames.pettostudio.net/RecommendGames.aspx?action=getgamelist&getCount=1000&pageNumber=0&rd={0}&auth={1}", timeAuth, cryptStr));
                List<GameModel> result = JsonHelper.DeserializeObjectFromJson<List<GameModel>>(Encryption.Decrypt(result_str));

                foreach (var gameModel in result)
                {
                    gameModel.GameType = HttpUtility.UrlDecode(gameModel.GameType);
                    gameModel.GameDetails = HttpUtility.HtmlDecode(HttpUtility.UrlDecode(gameModel.GameDetails));
                    gameModel.GameName = HttpUtility.HtmlDecode(HttpUtility.UrlDecode(gameModel.GameName));
                    gameModel.LogoPath = HttpUtility.UrlDecode(gameModel.LogoPath);
                    gameModel.HeadImage = HttpUtility.UrlDecode(gameModel.HeadImage);

                    gameModel.Images1 = HttpUtility.UrlDecode(gameModel.Images1);
                    gameModel.Images2 = HttpUtility.UrlDecode(gameModel.Images2);
                    gameModel.Images3 = HttpUtility.UrlDecode(gameModel.Images3);
                    gameModel.Images4 = HttpUtility.UrlDecode(gameModel.Images4);
                    gameModel.Images5 = HttpUtility.UrlDecode(gameModel.Images5);
                    gameModel.Images6 = HttpUtility.UrlDecode(gameModel.Images6);
                    gameModel.Images7 = HttpUtility.UrlDecode(gameModel.Images7);
                    gameModel.Images8 = HttpUtility.UrlDecode(gameModel.Images8);
                }

                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static void UpdateOrderForGame(List<GameModel> gameList)
        {
            try
            {
                string timeAuth = DateTime.Now.Millisecond.ToString();
                string cryptStr = Encryption.Encrypt("updateorderforgame" + timeAuth);
                string dataString = string.Format("action=updateorderforgame&gameListJson={0}&rd={1}&auth={2}",
                                               JsonHelper.SerializerToJson(gameList), timeAuth, cryptStr);
                string result = HttpHelper.HTTP_POST("http://recommendgames.pettostudio.net/RecommendGames.aspx", dataString);

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

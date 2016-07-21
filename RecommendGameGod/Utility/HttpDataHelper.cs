using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using UtilityLibs;

namespace RecommendGameGod.Utility
{
    public class HttpDataHelper
    {
        public static void AddGame(GameModel gameModel)
        {
            try
            {
                string timeAuth = DateTime.Now.Millisecond.ToString();
                string cryptStr = Encryption.Encrypt("addgames" + timeAuth);

                gameModel.GameType = HttpUtility.UrlEncode(gameModel.GameType);
                gameModel.GameName = HttpUtility.UrlEncode(HttpUtility.HtmlEncode(gameModel.GameName));
                gameModel.GameDetails = HttpUtility.UrlEncode(HttpUtility.HtmlEncode(gameModel.GameDetails));
                gameModel.LogoPath = HttpUtility.UrlEncode(gameModel.LogoPath);
                gameModel.HeadImage = HttpUtility.UrlEncode(gameModel.HeadImage);

                gameModel.Images1 = HttpUtility.UrlEncode(gameModel.Images1);
                gameModel.Images2 = HttpUtility.UrlEncode(gameModel.Images2);
                gameModel.Images3 = HttpUtility.UrlEncode(gameModel.Images3);
                gameModel.Images4 = HttpUtility.UrlEncode(gameModel.Images4);
                gameModel.Images5 = HttpUtility.UrlEncode(gameModel.Images5);
                gameModel.Images6 = HttpUtility.UrlEncode(gameModel.Images6);
                gameModel.Images7 = HttpUtility.UrlEncode(gameModel.Images7);
                gameModel.Images8 = HttpUtility.UrlEncode(gameModel.Images8);

                string dataStr = string.Format("action={0}&gametype={1}&gamename={2}&version={3}&gameid={4}&pushername={5}&updatetime={6}&gamedetails={7}&logopath={8}&sourcetype={9}&downloadcount={10}"
                                               + "&price={11}&filesize={12}&starts={13}&headimage={14}&rating={15}&images1={16}&images2={17}&images3={18}&images4={19}&images5={20}&images6={21}&images7={22}&images8={23}&phoneversion={24}&rd={25}&auth={26}",
                                               "addgames", gameModel.GameType, gameModel.GameName, gameModel.Version, gameModel.GameID, gameModel.PusherName, gameModel.UpdateTime, gameModel.GameDetails, gameModel.LogoPath, gameModel.SourceType, gameModel.DownloadCount,
                                               gameModel.Price, gameModel.FileSize, gameModel.Starts, gameModel.HeadImage, gameModel.Rating, gameModel.Images1, gameModel.Images2, gameModel.Images3, gameModel.Images4, gameModel.Images5, gameModel.Images6, gameModel.Images7, gameModel.Images8, gameModel.PhoneVersion,
                                               timeAuth,cryptStr);

                string result = HttpHelper.HTTP_POST("http://recommendgames.pettostudio.net/RecommendGames.aspx", dataStr);
                //string result = HttpHelper.HTTP_POST("http://localhost:21422/RecommendGames.aspx", dataStr);
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

        public static void UploadImage(string imagePath)
        {
            WebClient wc = new WebClient();
            try
            {
                wc.UploadFile("http://recommendgames.pettostudio.net/UploadManager.aspx", imagePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetGameXMLInfo(string gameId)
        {
            WebClient wc = new WebClient();

            try
            {
                wc.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                wc.Headers.Add("Content-Type", "application/atom+xml");
                wc.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36");

                return wc.DownloadString(string.Format("http://marketplaceedgeservice.windowsphone.com/v8/catalog/apps/{0}?os=8.0.10211.0&cc=AU&lang=en-US", gameId));
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

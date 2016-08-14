using Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityLibs;

namespace TestControl
{
    class Program
    {
        static void Main(string[] args)
        {
            ReOrderGameList();

            List<NewsInfoForJson> n1 = HtmlHelper.GetNewsInfoList("http://www.windowscentral.com/windows-phone");
            List<NewsInfoForJson> n2 = HtmlHelper.GetNewsInfoList("http://www.windowscentral.com/apps?utm_medium=navbar&utm_campaign=navigation&utm_source=wc");
            List<NewsInfoForJson> n3 = HtmlHelper.GetNewsInfoList("http://www.windowscentral.com/games?utm_medium=navbar&utm_campaign=navigation&utm_source=wc");
            List<NewsInfoForJson> n4 = HtmlHelper.GetNewsInfoList("http://www.windowscentral.com/lumia-950-xl/home?utm_medium=navbar&utm_campaign=navigation&utm_source=wc");
            List<NewsInfoForJson> n5 = HtmlHelper.GetNewsInfoList("http://www.windowscentral.com/lumia-950?utm_medium=navbar&utm_campaign=navigation&utm_source=wc");
            List<NewsInfoForJson> n6 = HtmlHelper.GetNewsInfoList("http://www.windowscentral.com/surface?utm_medium=navbar&utm_campaign=navigation&utm_source=wc");
            List<NewsInfoForJson> n7 = HtmlHelper.GetNewsInfoList("http://www.windowscentral.com/xbox?utm_medium=navbar&utm_campaign=navigation&utm_source=wc");
            List<NewsInfoForJson> n8 = HtmlHelper.GetNewsInfoList("http://www.windowscentral.com/windows-10?utm_medium=navbar&utm_campaign=navigation&utm_source=wc");
            List<NewsInfoForJson> n9 = HtmlHelper.GetNewsInfoList("http://www.windowscentral.com/microsoft-hololens?utm_medium=navbar&utm_campaign=navigation&utm_source=wc");
            List<NewsInfoForJson> n10 = HtmlHelper.GetNewsInfoList("http://www.windowscentral.com/microsoft-band-2?utm_medium=navbar&utm_campaign=navigation&utm_source=wc");

            AddNews(n1);
            AddNews(n2);
            AddNews(n3);
            AddNews(n4);
            AddNews(n5);
            AddNews(n6);
            AddNews(n7);
            AddNews(n8);
            AddNews(n9);
            AddNews(n10);
        }

        private static void AddNews(List<NewsInfoForJson> l)
        {
            foreach (var item in l)
            {
                try
                {
                    HttpDataHelper.AddNews(item);
                    Log(item.Title + ",OK!");
                }
                catch (Exception ex)
                {
                    Log(item.Title + ex.Message);
                }
            }
        }

        private static void ReOrderGameList()
        {
            try
            {
                List<GameModel> gameList = HttpDataHelper.GetGameList();

                if (gameList != null && gameList.Count > 0)
                {
                    List<int> tempIndex = new List<int>();
                    for (int i = 0; i < gameList.Count - 10; i++)
                    {
                        if (i < 10)
                        {
                            tempIndex.Add(gameList[i].Order);
                        }
                        gameList[i].Order = gameList[i + 10].Order;
                    }

                    for (int i = 0; i < 10; i++)
                    {
                        gameList[gameList.Count - i - 1].Order = tempIndex[i];
                    }

                    List<GameModel> gl_t = new List<GameModel>();

                    foreach (var item in gameList)
                    {
                        gl_t.Add(new GameModel { ID = item.ID, Order = item.Order, IsTopmost = item.IsTopmost });
                    }

                    HttpDataHelper.UpdateOrderForGame(gl_t);
                }

                Log("更新列表成功");
            }

            catch (Exception ex)
            {
                Log("更新列表失败," + ex.Message);
            }
        }

        private static void Log(string info)
        {
            try
            {
                Console.WriteLine(info);

                string logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log");

                if (!Directory.Exists(logDir))
                {
                    Directory.CreateDirectory(logDir);
                }

                using (StreamWriter sw = new StreamWriter(Path.Combine(logDir, string.Format("{0}_log.txt", DateTime.Now.ToString("yyyy-MM-dd"))), true))
                {
                    sw.WriteLine(string.Format("{0},{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), info));
                }
            }
            catch { }
        }
    }
}

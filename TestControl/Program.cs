using Models;
using System;
using System.Collections.Generic;
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

            Console.ReadKey();
        }

        private static void AddNews(List<NewsInfoForJson> l)
        {
            foreach (var item in l)
            {
                try
                {
                    HttpDataHelper.AddNews(item);
                    Console.WriteLine(item.Title + ",OK!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(item.Title + ex.Message);
                }
            }
        }
    }
}

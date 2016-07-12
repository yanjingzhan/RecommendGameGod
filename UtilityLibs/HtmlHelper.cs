using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace UtilityLibs
{
    public class HtmlHelper
    {
        private static readonly string HOST = "http://www.windowscentral.com/";

        public static List<NewsInfoForJson> GetNewsInfoList(string url)
        {
            HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = web.Load(url);

            HtmlAgilityPack.HtmlNodeCollection clooection = doc.DocumentNode.SelectNodes("//div[@class=\"grid_item visor-article-teaser list_default\"]");

            List<NewsInfoForJson> result = new List<NewsInfoForJson>();
            if (clooection.Count > 0)
            {
                foreach (var c in clooection)
                {
                    NewsInfoForJson news_t = new NewsInfoForJson();
                    HtmlAgilityPack.HtmlNode imageNode = c.SelectSingleNode(".//img");
                    if (imageNode != null)
                    {
                        string image_t = imageNode.GetAttributeValue("src", "");
                        news_t.Titlepic = image_t.StartsWith("http") ? image_t : HOST + image_t.TrimStart('/');
                    }

                    HtmlAgilityPack.HtmlNode urlNode = c.SelectSingleNode(".//a[@class='grid_img']");
                    if (urlNode != null)
                    {
                        string url_t = urlNode.GetAttributeValue("href", "");
                        news_t.befrom = url_t.StartsWith("http") ? url_t : HOST + url_t.TrimStart('/');
                    }

                    HtmlAgilityPack.HtmlNode timeNode = c.SelectSingleNode(".//span[@class='grid_time']");
                    if (timeNode != null)
                    {
                        news_t.NewsTime = timeNode.InnerText;
                    }

                    HtmlAgilityPack.HtmlNode titleNode = c.SelectSingleNode(".//*[@class='grid_title']");
                    if (titleNode != null)
                    {
                        news_t.Title = HttpUtility.HtmlDecode(titleNode.InnerText);
                    }

                    news_t.NewsForm = "news";

                    GetContentText(news_t.befrom, ref news_t);
                    news_t.Onclick = new Random().Next(100, 2000).ToString();

                    result.Add(news_t);
                }
            }

            return result;
        }

        public static void GetContentText(string url, ref NewsInfoForJson newsModel_t)
        {
            HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = web.Load(url);

            HtmlAgilityPack.HtmlNode contentNode = doc.DocumentNode.SelectSingleNode("//div[@class=\"field field-name-body field-type-text-with-summary field-label-hidden\"]/div[@class=\"field-items\"]/div[@class=\"field-item even\"]");
            if (contentNode != null)
            {
                newsModel_t.newstext = contentNode.OuterHtml;
            }

            HtmlAgilityPack.HtmlNodeCollection nodeCollection = doc.DocumentNode.SelectNodes("//a[@class=\"cta large\"]");
            if (nodeCollection != null && nodeCollection.Count > 0)
            {
                foreach (var node in nodeCollection)
                {
                    string url_t = HttpUtility.HtmlDecode(HttpUtility.UrlDecode(node.GetAttributeValue("href", "")));
                    if (url_t.Contains("www.microsoft.com") && url_t.Contains("store") && url_t.Contains("apps"))
                    {
                        int index_0 = url_t.IndexOf("&url=https") + 5;
                        int index_1 = url_t.LastIndexOf("&token=");
                         string fileName_t = HttpUtility.UrlDecode(
                            index_1 > 0 ? url_t.Substring(index_0, index_1 - index_0) : url_t.Substring(index_0));

                        int index_2 = fileName_t.LastIndexOf("&ourl=http");
                        if(index_2 > 0)
                        {
                            newsModel_t.Filename = fileName_t.Substring(0, index_2);
                        }
                        
                        newsModel_t.NewsForm = "pingce";
                    }
                }
            }
        }
    }
}

using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RecommendGameGod.Utility
{
    public class XMLHelper
    {
        public static GameInfoInStore GetGameInfoInStore(string storeXML)
        {
            GameInfoInStore result = new GameInfoInStore();
            result.screenshots = new List<string>();

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(storeXML.Replace("a:","").Replace("xmlns:a=\"http://www.w3.org/2005/Atom\" xmlns:os=\"http://a9.com/-/spec/opensearch/1.1/\" xmlns=\"http://schemas.zune.net/catalog/apps/2008/02\"",""));


            //System.Xml.XmlNamespaceManager nsmanager = new System.Xml.XmlNamespaceManager(doc.NameTable);
            //nsmanager.AddNamespace("a", "http://www.w3.org/2005/Atom");
            //nsmanager.AddNamespace("os", "http://a9.com/-/spec/opensearch/1.1/");
            //nsmanager.AddNamespace(" ", "http://schemas.zune.net/catalog/apps/2008/02");

            XmlElement rootElem = doc.DocumentElement;
            result.Content =(rootElem.SelectSingleNode("content")).InnerText;
            result.Title = (rootElem.SelectSingleNode("title")).InnerText;
            result.Updated = (rootElem.SelectSingleNode("releaseDate")).InnerText;
            result.Category = (rootElem.SelectNodes("categories/category")[1] as XmlElement).SelectSingleNode("title").InnerText;
            result.Publisher = (rootElem.SelectSingleNode("publisherId")).InnerText;
            result.Image = string.Format("http://cdn.marketplaceimages.windowsphone.com/v3.2/image/{0}?width=320&height=320&resize=true&contenttype=image/png",
                (rootElem.SelectSingleNode("image/id")).InnerText.Replace("urn:uuid:", ""));

            result.PackageSize = (double.Parse((rootElem.SelectSingleNode("entry/packageSize")).InnerText) / 1024.00d / 1024.00d).ToString("0.00");
            result.Version = ((rootElem.SelectSingleNode("entry/version")).InnerText);
                                    
            foreach (XmlNode item in rootElem.SelectNodes("screenshots/screenshot"))
            {
                result.screenshots.Add(string.Format("http://cdn.marketplaceimages.windowsphone.com/v3.2/image/{0}?width=320&height=320",
                    item.SelectSingleNode("id").InnerText.Replace("urn:uuid:", "")));
            }

            foreach (XmlNode item in rootElem.SelectNodes("offers/offer/clientTypes/clientType"))
            {
                result.ClientTypes += item.InnerText + "+";
            }

            return result;
        }
    }
}

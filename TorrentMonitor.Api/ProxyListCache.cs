using System.Collections.Generic;
using System.Linq;
using System.Net;
using HtmlAgilityPack;
using Newtonsoft.Json;
using StackExchange.Redis;
using TorrentMonitor.Api.Modules;

namespace TorrentMonitor.Api
{
   public  class ProxyListFetcher
    {

       private List<PBProxy> FetchRedisProxy()
        {
            var cm = ConnectionMultiplexer.Connect("torrenttracker.redis.cache.windows.net,ssl=true,password=i9zpOqD1ZOIBg36e/SdcgLwuGwI5q3XM2REIz1awxBw=");
            var db = cm.GetDatabase();
           var key = db.StringGet("ProxyList");
           if (key.IsNullOrEmpty)
               return null;
            return JsonConvert.DeserializeObject<List<PBProxy>>(key);
        }


      private void PopoulateRedis(string proxyList)
       {
            var cm = ConnectionMultiplexer.Connect("torrenttracker.redis.cache.windows.net,ssl=true,password=i9zpOqD1ZOIBg36e/SdcgLwuGwI5q3XM2REIz1awxBw=");
            var db = cm.GetDatabase();
            
          db.StringSet("ProxyList", proxyList);
        }


       public List<PBProxy> Fetch()
       {


           var proxylist = FetchRedisProxy();
           if (proxylist == null)
           {
               proxylist = ScrapeFromWeb();
               PopoulateRedis(JsonConvert.SerializeObject(proxylist));
           }

           return proxylist;
        }


       private List<PBProxy> ScrapeFromWeb()
       {

           var proxies = new List<PBProxy>();

           using (var client = new WebClient())
           {

               var html = new HtmlWeb().Load("http://proxybay.info/");
               var nodes = html.DocumentNode.SelectNodes("//tr");
               foreach (var a in nodes)
               {

                   var site = a.SelectNodes("td[contains(@class,'site')]");

                   if (site != null)
                   {
                       var proxy = new PBProxy();
                       var url = site.FindFirst("a").Attributes["href"].Value;
                       var status =
                           ResolveStatus(
                               a.SelectNodes("td[contains(@class,'status')]").FindFirst("img").Attributes["src"].Value);
                       var speed = a.SelectNodes("td[contains(@class,'speed')]").Nodes().First().InnerText;

                       proxy.Url = url;
                       proxy.Status = status;
                       proxy.Speed = speed;

                       proxies.Add(proxy);
                   }
               }


           }
           return proxies;
       }

       private bool ResolveStatus(string img)
        {
            if (img.Contains("up"))
                return true;
            return false;
        }


    }
}

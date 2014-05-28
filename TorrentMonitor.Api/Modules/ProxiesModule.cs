using System.Collections.Generic;
using System.Linq;
using System.Net;
using HtmlAgilityPack;
using Nancy;

namespace TorrentMonitor.Api.Modules
{
    public class ProxiesModule : NancyModule
    {
        public ProxiesModule()
        {
            Get["/"] = x => GetProxies();
        }

        private Response GetProxies()
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

            return Response.AsJson(proxies);
        }

        private bool ResolveStatus(string img)
        {
            if (img.Contains("up"))
                return true;
            return false;
        }




    }
}

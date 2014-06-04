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
            var proxies = new ProxyListFetcher().Fetch();

            return Response.AsJson(proxies);
        }

      }
}

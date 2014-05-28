using System;
using Nancy.Hosting.Self;
using Nancy.TinyIoc;

namespace TorrentMonitor.Api
{
    internal class ApiService
    {
        public TinyIoCContainer Container { get; set; }
        private NancyHost host { get; set; }

        public ApiService()
        {
            var config = new HostConfiguration();
            var urls = new UrlReservations { CreateAutomatically = true };
            config.UrlReservations = urls;
            var uri = new Uri("http://localhost:8643");
            host = new NancyHost(config, uri);
        }


        public void Start()
        {
            RegisterDependencies();
            host.Start();
        }

        public void Stop()
        {
            host.Stop();
        }

        private void RegisterDependencies()
        {
            Container = TinyIoCContainer.Current;
        }
    }
}

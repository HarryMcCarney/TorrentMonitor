using Topshelf;

namespace TorrentMonitor.Api
{
    class Program
    {
        static void Main(string[] args)
        {

            HostFactory.Run(x =>
            {
                x.Service<ApiService>(s =>
                {
                    s.ConstructUsing(name => new ApiService());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                //x.RunAs("BizIntell\\Harry", "Popov2010");

                const string serviceName = "PBProxy_Api";
                x.SetDescription(serviceName);
                x.SetDisplayName(serviceName);
                x.SetServiceName(serviceName);
            });           
        }
    }
}

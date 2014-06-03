using System;
using StackExchange.Redis;

namespace ConsoleApplication1
{

    class Program
    {
        static void Main(string[] args)
        {
            var cm = ConnectionMultiplexer.Connect("torrenttracker.redis.cache.windows.net,ssl=true,password=i9zpOqD1ZOIBg36e/SdcgLwuGwI5q3XM2REIz1awxBw=");
            var db = cm.GetDatabase();

            db.StringSet("testing", "123");
            var key = db.StringGet("testing");

            Console.WriteLine(key);
            Console.ReadLine();
        }
    }


    public class ProxyListFetcher
    {


    }

}

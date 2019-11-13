using Microsoft.Owin.Hosting;
using System;
using Video.Lib;

namespace Video.ConsoleApplication
{
    class Program
    {
        //private static string baseUrl = @"D:\\video\";

        static void Main(string[] args)
        {

           // var jsonContent = httpClient.GetWebClient("http://api.bilibili.com/playurl?aid=7539335");
            //var json = httpClient.Deserialize<Model>(jsonContent);

            //    string baseAddress = "http://localhost:9000/";
            //    using (WebApp.Start<Startup>(url: baseAddress))
            //    {
            //        HttpClient client = new HttpClient();
            //        var response = client.GetAsync(baseAddress + "api/values").Result;
            //        Console.WriteLine(response);
            //        Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            //    }

            //    Console.ReadKey();


            //var pic = "http://i2.hdslb.com/bfs/archive/75c17c1b6a07d08de0206d573ec5567cce5dad66.jpg";

            //httpClient.download(pic, baseUrl + 3 + ".jpg");

            string baseAddress = "http://localhost:9000/";
            WebApp.Start<Video.Lib.Startup>(url: baseAddress);
            Console.WriteLine(string.Format("服务已启动,服务地址:{0}", baseAddress));
            Console.ReadKey();
        }
    }
}

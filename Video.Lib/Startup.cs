using Microsoft.Owin;
using Owin;
using System.Web.Http;
using System.Web.Http.Cors;

[assembly: OwinStartup(typeof(Video.Lib.Startup))]
namespace Video.Lib
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // 有关如何配置应用程序的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkID=316888
            HttpConfiguration config = new HttpConfiguration();
            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{Controller}/{id}",
            //    defaults: new
            //    {
            //        id = RouteParameter.Optional
            //    });

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.EnableCors(new EnableCorsAttribute("*", "*", "*"));




            app.UseWebApi(config);
        }
    }
}

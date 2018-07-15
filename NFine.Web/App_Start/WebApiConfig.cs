using System.Web.Http;
using System.Web.Http.Cors;

namespace NFine.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //启用支持跨域
            config.EnableCors(new EnableCorsAttribute("*", "*", "*"));

            // 启用Web API特性路由
            config.MapHttpAttributeRoutes();


            //#region Web API Routes
            ////// Web API Session Enabled Route Configurations
            ////RouteTable.Routes.MapHttpRoute(
            ////    name: "SessionsRoute",
            ////    routeTemplate: "api/sessions/{controller}/{id}",
            ////    defaults: new { id = RouteParameter.Optional }
            ////).RouteHandler = new SessionEnabledHttpControllerRouteHandler();

            //// Web API Stateless Route Configurations
            //RouteTable.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);
            //#endregion

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}

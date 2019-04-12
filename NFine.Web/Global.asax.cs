using NFine.Code;
using System.Web;
using System.Web.Caching;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;

namespace NFine.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public override void Init()
        {
            this.PostAuthenticateRequest += (sender, e) => HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
            base.Init();
        }

        /// <summary>
        /// 启动应用程序
        /// </summary>
        protected void Application_Start()
        {
            NFine.Application.AutoMapper.Configuration.Configure();//注册DTO映射(AutoMapper)
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //DBConnection.Encrypt = true;//开启数据库连接字符串加密功能
            var connectionString = DBConnection.connectionString;
            //启动数据库的数据缓存依赖功能    
            SqlCacheDependencyAdmin.EnableNotifications(connectionString);
            string[] tables = { "Web_Attachment", "Web_Content", "Web_Node", "Web_FriendLinks", "Sys_Items", "Sys_ItemsDetail", "Sys_Module", "Sys_ModuleButton", "Sys_ModuleForm", "Sys_Role", "Sys_RoleAuthorize", "Sys_User", "Sys_UserLogOn", "Sys_Area", "Sys_Log", "Sys_Organize", "Sys_BackupServer", "Sys_FileBackup", "Web_Picture" };
            //启用数据表缓存
            SqlCacheDependencyAdmin.EnableTableForNotifications(connectionString, tables);

            //注册缓存依赖的服务代码：在vs命令行输入：aspnet_regsql -S . -U sa -P 123 -ed -d NFineBase -et -t Web_Content
            //-S localhost：数据库地址
            //-U sa：数据库登录名
            //-P 123456：数据库登录密码
            //-d wcfDemo：数据库的名称
            //-t user：表名称(小写)


            //消息队列处理，需要服务器部署redis
            //MessageQueueConfig.RegisterExceptionLogQueue();
        }
    }
}
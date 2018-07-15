using NFine.Application;
using NFine.Application.SystemSecurity;
using NFine.Code;
using NFine.Domain.Entity.SystemSecurity;
using System;
using System.Web;
using System.Web.Mvc;

namespace NFine.Web
{
    public class HandlerErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            try
            {
                WriteLog(context);
            }
            catch (System.Exception)
            {

            }

            base.OnException(context);
            context.ExceptionHandled = true;
            context.HttpContext.Response.StatusCode = 200;
            context.Result = new ContentResult { Content = new AjaxResult { state = ResultType.error.ToString(), message = context.Exception.Message }.ToJson() };
        }
        private void WriteLog(ExceptionContext context)
        {
            if (context == null)
                return;
            var log = LogFactory.GetLogger(context.Controller.ToString());
            log.Error(context.Exception);
            Exception Error = context.Exception;
            string strMessage = string.Empty;
            if (Error.InnerException == null)
            {
                strMessage = Error.Message;
            }
            else
            {
                strMessage = Error.InnerException.Message;
            }

            new LogApp().WriteDbLog(new LogEntity
            {
                F_ModuleId = "HandlerError",
                F_ModuleName = HttpContext.Current.Request.RawUrl,
                F_Type = DbLogType.Exception.ToString(),
                F_Account = OperatorProvider.Provider.GetCurrent().UserCode,
                F_NickName = OperatorProvider.Provider.GetCurrent().UserName,
                F_Result = true,
                F_Description = strMessage,
            });
        }
    }
}
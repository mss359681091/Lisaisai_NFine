using NFine.Application;
using NFine.Application.SystemManage;
using NFine.Application.SystemSecurity;
using NFine.Code;
using NFine.Domain.Entity.SystemManage;
using NFine.Domain.Entity.SystemSecurity;
using NFine.Redis;
using NFine.Web.Infrastructure;
using System.Collections.Generic;
using System.Web.Mvc;

namespace NFine.Web
{
    [HandlerLogin]
    public abstract class ControllerBase : Controller
    {
        private OrganizeApp organizeApp = new OrganizeApp();
        private RoleApp roleApp = new RoleApp();
        private DutyApp dutyApp = new DutyApp();


        public Log FileLog
        {
            get { return LogFactory.GetLogger(this.GetType().ToString()); }
        }
        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult Form()
        {
            return View();
        }
        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult Details()
        {
            return View();
        }
        protected virtual ActionResult Success(string message)
        {
            return Content(new AjaxResult { state = ResultType.success.ToString(), message = message }.ToJson());
        }
        protected virtual ActionResult Success(string message, object data)
        {
            return Content(new AjaxResult { state = ResultType.success.ToString(), message = message, data = data }.ToJson());
        }
        protected virtual ActionResult Error(string message)
        {
            return Content(new AjaxResult { state = ResultType.error.ToString(), message = message }.ToJson());
        }

        protected virtual void OperateLog(string errMsg, string moduleName, string moduleId, DbLogType type)
        {
            string cacheType = Configs.GetValue("CacheType");//缓存类型
            switch (cacheType)
            {
                case "Redis":
                    errMsg = errMsg.Replace(";", "");
                    errMsg = moduleId + ";" + moduleName + ";" + type.ToString() + ";" + errMsg + ";" + OperatorProvider.Provider.GetCurrent().UserCode + ";" + OperatorProvider.Provider.GetCurrent().UserName;
                    RedisCache.EnqueueItemOnList(RedisTypeEnum.OperateLog.ToString(), errMsg);//操作消息入队   
                    break;
                case "WebCache":
                    if (!string.IsNullOrEmpty(errMsg))
                    {
                        new LogApp().WriteDbLog(new LogEntity
                        {
                            F_ModuleId = moduleId,
                            F_ModuleName = moduleName,
                            F_Type = type.ToString(),
                            F_Account = OperatorProvider.Provider.GetCurrent().UserCode,
                            F_NickName = OperatorProvider.Provider.GetCurrent().UserName,
                            F_Result = true,
                            F_Description = errMsg,
                        });
                    }
                    break;
                default:

                    break;
            }
        }

        #region 绑定下拉菜单
        /// <summary>
        /// 绑定公司
        /// </summary>
        /// <param name="IsDefault"></param>
        /// <param name="Value"></param>
        public void BindOrganizeId(string Value, bool IsDefault = false)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            if (IsDefault)
            {
                list.Add(new SelectListItem() { Value = "", Text = "请选择" });
            }
            var data = organizeApp.GetList();
            foreach (OrganizeEntity item in data)
            {
                list.Add(new SelectListItem() { Value = item.F_Id, Text = item.F_FullName });
            }
            ViewBag.F_OrganizeId = new SelectList(list, "Value", "Text", Value);
        }

        /// <summary>
        /// 绑定部门
        /// </summary>
        /// <param name="IsDefault"></param>
        /// <param name="Value"></param>
        public void BindDepartmentId(string Value, bool IsDefault = false)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            if (IsDefault)
            {
                list.Add(new SelectListItem() { Value = "", Text = "请选择" });
            }
            var data = organizeApp.GetList();
            foreach (OrganizeEntity item in data)
            {
                list.Add(new SelectListItem() { Value = item.F_Id, Text = item.F_FullName });
            }
            ViewBag.F_DepartmentId = new SelectList(list, "Value", "Text", Value);
        }

        /// <summary>
        /// 绑定角色
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="IsDefault"></param>
        public void BindRoleId(string Value, bool IsDefault = false)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            if (IsDefault)
            {
                list.Add(new SelectListItem() { Value = "", Text = "请选择" });
            }
            var data = roleApp.GetList();
            foreach (RoleEntity item in data)
            {
                list.Add(new SelectListItem() { Value = item.F_Id, Text = item.F_FullName });
            }
            ViewBag.F_RoleId = new SelectList(list, "Value", "Text", Value);
        }

        /// <summary>
        /// 绑定职责
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="IsDefault"></param>
        public void BindDutyId(string Value, bool IsDefault = false)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            if (IsDefault)
            {
                list.Add(new SelectListItem() { Value = "", Text = "请选择" });
            }
            var data = dutyApp.GetList();
            foreach (RoleEntity item in data)
            {
                list.Add(new SelectListItem() { Value = item.F_Id, Text = item.F_FullName });
            }
            ViewBag.F_DutyId = new SelectList(list, "Value", "Text", Value);
        }
        #endregion
    }
}

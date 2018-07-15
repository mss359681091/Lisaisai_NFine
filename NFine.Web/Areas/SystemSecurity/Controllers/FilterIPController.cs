/*******************************************************************************
 * Copyright © 2016 NFine.Framework 版权所有
 * Author: Lss
 * Description: NFine快速开发平台
 * Website：http://blog.csdn.net/mss359681091
*********************************************************************************/
using NFine.Application.SystemSecurity;
using NFine.Code;
using NFine.Domain.Entity.SystemSecurity;
using System.Web.Mvc;

namespace NFine.Web.Areas.SystemSecurity.Controllers
{
    public class FilterIPController : ControllerBase
    {
        private FilterIPApp filterIPApp = new FilterIPApp();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string queryJson)
        {
            var data = new
            {
                rows = filterIPApp.GetList(pagination, queryJson),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = filterIPApp.GetForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(FilterIPEntity filterIPEntity, string keyValue)
        {
            filterIPApp.SubmitForm(filterIPEntity, keyValue);
            if (!string.IsNullOrEmpty(keyValue))
            {
                base.ErrLog("编辑控制：F_Id:" + keyValue, "/SystemSecurity/FilterIP/SubmitForm", "访问控制", Application.DbLogType.Update);
            }
            else
            {
                base.ErrLog("新增控制", "/SystemSecurity/FilterIP/SubmitForm", "访问控制", Application.DbLogType.Create);
            }
            return Success("操作成功。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            filterIPApp.DeleteForm(keyValue);
            base.ErrLog("删除控制：" + keyValue, "/SystemSecurity/FilterIP/DeleteForm", "删除控制", Application.DbLogType.Delete);
            return Success("删除成功。");
        }
    }
}

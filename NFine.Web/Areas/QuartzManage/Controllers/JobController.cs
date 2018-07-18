using NFine.Application.QuartzManage;
using NFine.Code;
using NFine.Domain.Entity.QuartzManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NFine.Web.Areas.QuartzManage.Controllers
{
    public class JobController : ControllerBase
    {
        private CustomerJobInfoApp custApp = new CustomerJobInfoApp();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string keyword)
        {
            var data = new
            {
                rows = custApp.GetList(pagination, keyword),
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
            var data = custApp.GetForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(CustomerJobInfoEntity jobInfoEntity, string keyValue)
        {
            custApp.SubmitForm(jobInfoEntity, keyValue);
            return Success("操作成功。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            custApp.DeleteForm(keyValue);
            return Success("删除成功。");
        }


        #region 运行任务
        public ActionResult RunTask(string ids)
        {
            custApp.RunTask(ids);
            return Success("任务已运行。");
        }
        #endregion

        #region 暂停任务
        public ActionResult PauseTask(string ids)
        {
            custApp.PauseTask(ids);
            return Success("任务已暂停。");
        }
        #endregion

        #region 恢复任务
        public ActionResult ResumeTask(string ids)
        {
            custApp.ResumeJob(ids);
            return Success("任务已恢复。");
        }
        #endregion

        #region 删除任务
        public ActionResult DelTask(string ids)
        {
            custApp.DelTask(ids);
            return Success("任务已删除。");
        }
        #endregion

        #region 彻底删除任务
        public ActionResult DhysicallyDelTask(string ids)
        {
            custApp.DhysicallyDel(ids);
            return Success("任务已彻底删除。");
        }
        #endregion

    }
}
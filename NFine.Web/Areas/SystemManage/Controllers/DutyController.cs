/*******************************************************************************
 * Copyright © 2016 NFine.Framework 版权所有
 * Author: Lss
 * Description: NFine快速开发平台
 * Website：http://blog.csdn.net/mss359681091
*********************************************************************************/
using NFine.Application.SystemManage;
using NFine.Code;
using NFine.Domain.Entity.SystemManage;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace NFine.Web.Areas.SystemManage.Controllers
{
    public class DutyController : ControllerBase
    {
        private DutyApp dutyApp = new DutyApp();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(string keyword)
        {
            var data = dutyApp.GetList(keyword);
            return Content(data.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = dutyApp.GetForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(RoleEntity roleEntity, string keyValue)
        {
            dutyApp.SubmitForm(roleEntity, keyValue);
            if (!string.IsNullOrEmpty(keyValue))
            {
                base.OperateLog("编辑：F_Id:" + keyValue, "/SystemManage/Duty/Form", "岗位管理", Application.DbLogType.Update);
            }
            else
            {
                base.OperateLog("新建：F_Id:" + roleEntity.F_FullName, "/SystemManage/Duty/Form", "岗位管理", Application.DbLogType.Create);
            }
            return Success("操作成功。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            dutyApp.DeleteForm(keyValue);
            base.OperateLog("删除：F_Id:" + keyValue, "/SystemManage/Duty/DeleteForm", "岗位管理", Application.DbLogType.Delete);
            return Success("删除成功。");
        }
    }
}

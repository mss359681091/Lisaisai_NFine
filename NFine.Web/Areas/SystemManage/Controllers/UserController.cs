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
    public class UserController : ControllerBase
    {
        private UserApp userApp = new UserApp();
        private UserLogOnApp userLogOnApp = new UserLogOnApp();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string keyword)
        {
            var data = new
            {
                rows = userApp.GetList(pagination, keyword),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTreeSelectJson()
        {
            var data = userApp.GetList();
            var treeList = new List<TreeSelectModel>();
            foreach (UserEntity item in data)
            {
                TreeSelectModel treeModel = new TreeSelectModel();
                treeModel.id = item.F_Id;
                treeModel.text = item.F_RealName;
                treeModel.parentId = "0";
                treeModel.data = item;
                treeList.Add(treeModel);
            }
            return Content(treeList.TreeSelectJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = userApp.GetForm(keyValue);
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(UserEntity userEntity, UserLogOnEntity userLogOnEntity, string keyValue)
        {
            userApp.SubmitForm(userEntity, userLogOnEntity, keyValue);
            if (!string.IsNullOrEmpty(keyValue))
            {
                base.ErrLog("用户编辑：F_Id:" + keyValue, "/SystemManage/User/SubmitForm", "用户管理", Application.DbLogType.Update);
            }
            else
            {
                base.ErrLog("新增用户：" + userEntity.F_Account, "/SystemManage/User/SubmitForm", "用户管理", Application.DbLogType.Create);
            }
            return Success("操作成功。");
        }

        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            userApp.DeleteForm(keyValue);
            base.ErrLog("用户删除：F_Id:" + keyValue, "/SystemManage/User/DeleteForm", "用户管理", Application.DbLogType.Delete);
            return Success("删除成功。");
        }

        [HttpGet]
        public ActionResult RevisePassword()
        {
            return View();
        }

        [HttpPost]
        [HandlerAjaxOnly]
        //[HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitRevisePassword(string userPassword, string keyValue)
        {
            userLogOnApp.RevisePassword(userPassword, keyValue);
            base.ErrLog("重置密码：F_Id:" + keyValue, "/SystemManage/User/SubmitRevisePassword", "用户管理", Application.DbLogType.Submit);
            return Success("重置密码成功。");
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DisabledAccount(string keyValue)
        {
            UserEntity userEntity = new UserEntity();
            userEntity.F_Id = keyValue;
            userEntity.F_EnabledMark = false;
            userApp.UpdateForm(userEntity);
            base.ErrLog("禁用账户：F_Id:" + keyValue, "/SystemManage/User/DisabledAccount", "用户管理", Application.DbLogType.Submit);
            return Success("账户禁用成功。");
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult EnabledAccount(string keyValue)
        {
            UserEntity userEntity = new UserEntity();
            userEntity.F_Id = keyValue;
            userEntity.F_EnabledMark = true;
            userApp.UpdateForm(userEntity);
            base.ErrLog("启用账户：F_Id:" + keyValue, "/SystemManage/User/EnabledAccount", "用户管理", Application.DbLogType.Submit);
            return Success("账户启用成功。");
        }

        [HttpGet]
        public ActionResult Info()
        {
            ViewData["UserID"] = OperatorProvider.Provider.GetCurrent().UserId;
            base.BindOrganizeId("");//绑定公司
            base.BindDepartmentId("");//绑定部门
            base.BindRoleId("");//绑定角色
            base.BindDutyId("");//绑定职责
            return View();
        }

        [HttpPost]
        [HandlerAjaxOnly]
        public ActionResult GetUserName(string fid)
        {
            string strUsername = string.Empty;
            if (!string.IsNullOrEmpty(fid))
            {
                UserEntity ue = userApp.GetUserEntity(fid);
                strUsername = ue.F_RealName == "" ? ue.F_NickName : ue.F_RealName;
            }
            return Success(strUsername);
        }

        [HandlerAjaxOnly]
        public ActionResult GetUserNames(string fids)
        {
            string strUsernames = string.Empty;
            if (!string.IsNullOrEmpty(fids))
            {
                fids = StringHelper.GetStrJionChar(fids);
                strUsernames = userApp.GetUserNames(fids);//获取人员列表字符串
            }
            return Success(strUsernames);
        }


        [HandlerAjaxOnly]
        public JsonResult GetUserlstJson(string organizeId = "", string roleId = "", string dutyId = "", string departmentId = "", string keywords = "")
        {
            var data = userApp.GetListBySql(organizeId, roleId, dutyId, departmentId, keywords);
            return Json(data);
        }


        public ActionResult SelectUsers()
        {
            return View();
        }
        public ActionResult HasUsers()
        {
            return View();
        }
    }
}

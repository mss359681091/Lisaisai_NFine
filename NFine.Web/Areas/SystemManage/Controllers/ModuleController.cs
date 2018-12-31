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
    public class ModuleController : ControllerBase
    {
        private ModuleApp moduleApp = new ModuleApp();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTreeSelectJson()
        {
            var data = moduleApp.GetList();
            var treeList = new List<TreeSelectModel>();
            foreach (ModuleEntity item in data)
            {
                TreeSelectModel treeModel = new TreeSelectModel();
                treeModel.id = item.F_Id;
                treeModel.text = item.F_FullName;
                treeModel.parentId = item.F_ParentId;
                treeList.Add(treeModel);
            }
            return Content(treeList.TreeSelectJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTreeGridJson(string keyword)
        {
            var data = moduleApp.GetList();
            if (!string.IsNullOrEmpty(keyword))
            {
                data = data.TreeWhere(t => t.F_FullName.Contains(keyword));
            }
            var treeList = new List<TreeGridModel>();
            foreach (ModuleEntity item in data)
            {
                TreeGridModel treeModel = new TreeGridModel();
                bool hasChildren = data.Count(t => t.F_ParentId == item.F_Id) == 0 ? false : true;
                treeModel.id = item.F_Id;
                treeModel.isLeaf = hasChildren;
                treeModel.parentId = item.F_ParentId;
                treeModel.expanded = hasChildren;
                treeModel.entityJson = item.ToJson();
                treeList.Add(treeModel);
            }
            return Content(treeList.TreeGridJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = moduleApp.GetForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(ModuleEntity moduleEntity, string keyValue)
        {
            moduleApp.SubmitForm(moduleEntity, keyValue);
            if (!string.IsNullOrEmpty(keyValue))
            {
                base.OperateLog("菜单编辑：F_Id:" + keyValue, "/SystemManage/Module/SubmitForm", "菜单管理", Application.DbLogType.Update);
            }
            else
            {
                base.OperateLog("新增菜单：" + moduleEntity.F_FullName, "/SystemManage/Module/SubmitForm", "菜单管理", Application.DbLogType.Create);
            }
            return Success("操作成功。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            moduleApp.DeleteForm(keyValue);
            base.OperateLog("删除菜单：" + keyValue, "/SystemManage/Module/DeleteForm", "菜单管理", Application.DbLogType.Delete);
            return Success("删除成功。");
        }
    }
}

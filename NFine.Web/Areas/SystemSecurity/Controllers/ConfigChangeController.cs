using NFine.Code;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Configuration;
using System.Web.Mvc;

namespace NFine.Web.Areas.SystemSecurity.Controllers
{
    public struct ConfigChangeEntity
    {
        public String F_Key { get; set; }
        public String F_Value { get; set; }
    }
    public class ConfigChangeController : ControllerBase
    {
        //private SysConfigChangeApp configChangeApp = new SysConfigChangeApp();
        private ConfigChangeEntity configChangeEntity = new ConfigChangeEntity();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string queryJson)
        {
            List<ConfigChangeEntity> listConfigChangeEntity = new List<ConfigChangeEntity>();
            if (!queryJson.IsEmpty())
            {
                var queryParam = queryJson.ToJObject();
                string keyword = queryParam["keyword"].ToString().ToLower();
                foreach (string key in System.Configuration.ConfigurationManager.AppSettings.AllKeys)
                {
                    if (key.ToLower().Contains(keyword))
                    {
                        queryJson = string.Empty;
                        configChangeEntity.F_Key = key;
                        configChangeEntity.F_Value = System.Configuration.ConfigurationManager.AppSettings[key];
                        listConfigChangeEntity.Add(configChangeEntity);
                    }
                }
            }
            else
            {
                foreach (string key in System.Configuration.ConfigurationManager.AppSettings.AllKeys)
                {
                    queryJson = string.Empty;
                    configChangeEntity.F_Key = key;
                    configChangeEntity.F_Value = System.Configuration.ConfigurationManager.AppSettings[key];
                    listConfigChangeEntity.Add(configChangeEntity);
                }
            }
            return Content(listConfigChangeEntity.ToJson());
            //pagination.sidx = "F_CreatorTime desc";
            //pagination.sord = "desc";
            //var data = configChangeApp.GetList(pagination, queryJson);
            //return Content(data.ToJson());
        }

        //[HttpPost]
        //[HandlerAjaxOnly]
        //public ActionResult UpdateGridJson(Pagination pagination, string queryJson)
        //{
        //    foreach (string key in System.Configuration.ConfigurationManager.AppSettings.AllKeys)
        //    {
        //        queryJson = string.Empty;
        //        SysConfigChangeEntity configChangeEntity = new SysConfigChangeEntity();

        //        configChangeEntity.F_Key = key;
        //        configChangeEntity.F_Value = System.Configuration.ConfigurationManager.AppSettings[key];
        //        configChangeApp.SubmitForm(configChangeEntity, queryJson);
        //    }
        //    return Success("操作成功。");
        //}

        [HttpPost]
        [HandlerAjaxOnly]
        public ActionResult SubmitForm(ConfigChangeEntity configChangeEntity, string keyValue)
        {
            //configChangeApp.SubmitForm(configChangeEntity, keyValue);
            if (!string.IsNullOrEmpty(keyValue))
            {
                AccessAppSettings(configChangeEntity.F_Key, configChangeEntity.F_Value, 1);
            }
            else
            {
                AccessAppSettings(configChangeEntity.F_Key, configChangeEntity.F_Value, 0);
            }
            return Success("操作成功。");
        }
        private void AccessAppSettings(string key, string value, int caseNum)
        {
            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            switch (caseNum)
            {
                case 0:
                    //增加<add>元素
                    config.AppSettings.Settings.Add(key, value);
                    break;
                case 1:
                    //写入<add>元素的Value
                    config.AppSettings.Settings[key].Value = value;
                    break;
                case 2:
                    //删除<add>元素
                    config.AppSettings.Settings.Remove(key);
                    break;
            }
            //保存
            config.Save(ConfigurationSaveMode.Modified);
            //刷新
            System.Configuration.ConfigurationManager.RefreshSection("appSettings");
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            int keyV = -1;
            try
            {
                keyV = int.Parse(keyValue);
            }
            catch
            {

            }
            if (keyV >= 1)
            {
                keyV -= 1;
                configChangeEntity.F_Key = System.Configuration.ConfigurationManager.AppSettings.GetKey(keyV);
                configChangeEntity.F_Value = System.Configuration.ConfigurationManager.AppSettings[configChangeEntity.F_Key];
                return Content(configChangeEntity.ToJson());
            }
            return Success("请选择编辑项");
            //foreach (string key in System.Configuration.ConfigurationManager.AppSettings.AllKeys)
            //{
            //    queryJson = string.Empty;
            //    configChangeEntity.F_Key = key;
            //    configChangeEntity.F_Value = System.Configuration.ConfigurationManager.AppSettings[key];
            //    listConfigChangeEntity.Add(configChangeEntity);
            //}
            ////var data = configChangeApp.GetForm(keyValue);
            //return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            int keyV = -1;
            try
            {
                List<int> listNum = new List<int>();
                string[] listKeyValue = keyValue.Split(',');
                foreach (string str in listKeyValue)
                {
                    keyV = int.Parse(str);
                    int i = listNum.Count;
                    for (; i > 0; )
                    {
                        i--;
                        if (keyV < listNum[i])
                        {
                            break;
                        }
                    }
                    listNum.Insert(i, keyV);
                }                
                for (int i = 0; i < listNum.Count; i++)
                {
                    keyV = listNum[i];
                    if (keyV >= 1)
                    {
                        keyV -= 1;
                        configChangeEntity.F_Key = System.Configuration.ConfigurationManager.AppSettings.GetKey(keyV);
                        configChangeEntity.F_Value = System.Configuration.ConfigurationManager.AppSettings[configChangeEntity.F_Key];
                        AccessAppSettings(configChangeEntity.F_Key, configChangeEntity.F_Value, 2);
                    }
                }
                return Success("删除成功");
            }
            catch
            {
                return Success("删除失败");
            }
            return Success("请选择删除项");
            //if (!string.IsNullOrEmpty(keyValue))
            //{
            //    List<string> lstid = StringHelper.GetStrArray(keyValue, ',', false);
            //    for (int i = 0; i < lstid.Count; i++)
            //    {
            //        //SysConfigChangeEntity configChangeEntity = configChangeApp.GetForm(lstid[i]);
            //        //configChangeApp.DeleteForm(lstid[i]);
            //        AccessAppSettings(configChangeEntity.F_Key, configChangeEntity.F_Value, 2);
            //    }
            //    return Success("删除成功");
            //}
            //return Success("请选择删除项");
        }
    }
}

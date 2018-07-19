//-----------------------------------------------------------------------
// <copyright file=" CustomerJobInfo.cs" company="NFine">
// * Copyright (C) NFine.Framework  All Rights Reserved
// * version : 1.0
// * author  : NFine.Framework
// * FileName: CustomerJobInfo.cs
// * history : Created by T4 07/17/2018 21:29:17 
// </copyright>
//-----------------------------------------------------------------------
using NFine.Domain.Entity.QuartzManage;
using NFine.Domain.IRepository.QuartzManage;
using NFine.Repository.QuartzManage;
using NFine.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using Quartz.NetSchedulerManager;
using System.Text;
using System.Data.SqlClient;

namespace NFine.Application.QuartzManage
{
    public class CustomerJobInfoApp
    {
        private ICustomerJobInfoRepository service = new CustomerJobInfoRepository();
        JobHelper job = new JobHelper();

        public List<CustomerJobInfoEntity> GetList(Pagination pagination, string queryJson)
        {
            var expression = ExtLinq.True<CustomerJobInfoEntity>();
            var queryParam = queryJson.ToJObject();
            if (!queryParam["keyword"].IsEmpty())
            {
                string keyword = queryParam["keyword"].ToString();
                expression = expression.And(t => t.JobName.Contains(keyword) || t.JobGroupName.Contains(keyword) || t.TriggerName.Contains(keyword));
            }
            if (!queryParam["triggerState"].IsEmpty())
            {
                string triggerState = queryParam["triggerState"].ToString().Trim();
                expression = expression.And(t => t.TriggerState == triggerState);
            }
            return service.FindList(expression, pagination);
        }

        public CustomerJobInfoEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }

        public void Delete(CustomerJobInfoEntity entity)
        {
            service.Delete(entity);
        }

        public void DeleteForm(string keyValue)
        {
            service.Delete(t => t.F_Id == keyValue);
        }

        public void SubmitForm(CustomerJobInfoEntity entity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                job.ModifyJobCron(entity);//更改运行中CRON
                entity.Modify(keyValue);
                service.Update(entity);
            }
            else
            {
                entity.Create();
                if (!string.IsNullOrEmpty(entity.RequestUrl) && (string.IsNullOrEmpty(entity.WebApi) || entity.WebApi == "&nbsp;"))
                {
                    if (!entity.RequestUrl.Contains("http"))
                    {
                        entity.RequestUrl = "http://" + entity.RequestUrl;
                    }
                    var uri = new Uri(entity.RequestUrl);
                    entity.WebApi = "http://" + uri.Authority + "/api/Service/GetToken";//自动填充token验证
                }
                entity.DLLName = "Quartz.Net_JobBase.dll";
                entity.FullJobName = "Quartz.Net_JobBase.JobBase";
                entity.TriggerState = "-1";
                service.Insert(entity);
            }
        }

        public void UpdateForm(CustomerJobInfoEntity entity)
        {
            service.Update(entity);
        }

        /// <summary>
        /// 运行任务
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public void RunTask(string ids)
        {
            if (ids.Length > 0)
            {
                if (ids.Contains(","))
                {
                    string[] strids = StringHelper.GetStrArray(ids);
                    for (int i = 0; i < strids.Length; i++)
                    {
                        RunById(strids[i]);
                    }
                }
                else
                {
                    RunById(ids);
                }
            }
        }
        private void RunById(string keyValue)
        {
            CustomerJobInfoEntity model = GetForm(keyValue);
            job.RunJob(model);//运行job
            model.TriggerState = "0";//状态改为运行中
            UpdateForm(model);
        }

        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public void DelTask(string ids)
        {
            if (ids.Length > 0)
            {
                if (ids.Contains(","))
                {
                    string[] strids = StringHelper.GetStrArray(ids);
                    for (int i = 0; i < strids.Length; i++)
                    {
                        DelById(strids[i]);
                    }
                }
                else
                {
                    DelById(ids);
                }
            }
        }
        private void DelById(string keyValue)
        {
            CustomerJobInfoEntity model = GetForm(keyValue);
            model.Deleted = true;
            model.TriggerState = "5";//状态改为删除
            job.DeleteJob(model);//删除任务
            UpdateForm(model);
        }

        /// <summary>
        /// 彻底删除任务
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public void DhysicallyDel(string ids)
        {
            if (ids.Length > 0)
            {
                if (ids.Contains(","))
                {
                    string[] strids = StringHelper.GetStrArray(ids);
                    for (int i = 0; i < strids.Length; i++)
                    {
                        DhysicallyDelById(strids[i]);
                    }
                }
                else
                {
                    DhysicallyDelById(ids);
                }
            }
        }
        private void DhysicallyDelById(string keyValue)
        {
            CustomerJobInfoEntity model = GetForm(keyValue);
            Delete(model);
        }

        /// <summary>
        /// 暂停任务
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public void PauseTask(string ids)
        {
            if (ids.Length > 0)
            {
                if (ids.Contains(","))
                {
                    string[] strids = StringHelper.GetStrArray(ids);
                    for (int i = 0; i < strids.Length; i++)
                    {
                        PauseById(strids[i]);
                    }
                }
                else
                {
                    PauseById(ids);
                }
            }
        }
        private void PauseById(string keyValue)
        {
            CustomerJobInfoEntity model = GetForm(keyValue);
            model.TriggerState = "1";//状态改为暂停
            job.PauseJob(model);//暂停job
            UpdateForm(model);
        }

        /// <summary>
        /// 恢复任务
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public void ResumeJob(string ids)
        {
            if (ids.Length > 0)
            {
                if (ids.Contains(","))
                {
                    string[] strids = StringHelper.GetStrArray(ids);
                    for (int i = 0; i < strids.Length; i++)
                    {
                        ResumeById(strids[i]);
                    }
                }
                else
                {
                    ResumeById(ids);
                }
            }
        }
        private void ResumeById(string keyValue)
        {
            CustomerJobInfoEntity model = GetForm(keyValue);
            model.TriggerState = "0";//状态改为运行
            job.ResumeJob(model);//恢复job
            UpdateForm(model);
        }

        /// <summary>
        /// 检查名称是否已存在
        /// </summary>
        /// <param name="jobName"></param>
        /// <returns></returns>
        public bool CheckFiled(string jobName)
        {
            bool returnValue = false;
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  d.F_Id
                            FROM    Customer_JobInfo d
                            WHERE   1 = 1
                                    AND  d.JobName = @JobName
                                    AND (d.F_EnabledMark = 1 or d.F_EnabledMark is null)
                                    AND (d.F_DeleteMark = 0 or d.F_DeleteMark is null)");
            SqlParameter[] parameter =
            {
                 new SqlParameter("@JobName",jobName)
            };
            var result = SqlHelper.ExecuteScalarText(strSql.ToString(), parameter);
            if (result != null)
            {
                returnValue = true;
            }
            return returnValue;
        }
    }
}
//-----------------------------------------------------------------------
// <copyright file=" CustomerJobInfo.cs" company="NFine">
// * Copyright (C) NFine.Framework  All Rights Reserved
// * version : 1.0
// * author  : NFine.Framework
// * FileName: CustomerJobInfo.cs
// * history : Created by T4 07/17/2018 21:29:18 
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain.Entity.QuartzManage
{
    /// <summary>
    /// CustomerJobInfo Entity Model
    /// </summary>
    public class CustomerJobInfoEntity : IEntity<CustomerJobInfoEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        public string F_Id { get; set; }
        public String JobGroupName { get; set; }
        public String JobName { get; set; }
        public String TriggerName { get; set; }
        public String Cron { get; set; }
        public Int32? TriggerState { get; set; }
        public DateTime? JobStartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? PreTime { get; set; }
        public DateTime? NextTime { get; set; }
        public String Description { get; set; }
        public DateTime? CreateTime { get; set; }
        public String TriggerGroupName { get; set; }
        public String DLLName { get; set; }
        public String FullJobName { get; set; }
        public Boolean? Deleted { get; set; }
        public String Exception { get; set; }
        public String RequestUrl { get; set; }
        public String WebApi { get; set; }
        public int? F_SortCode { get; set; }
        public bool? F_DeleteMark { get; set; }
        public bool? F_EnabledMark { get; set; }
        public string F_Description { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public string F_CreatorUserId { get; set; }
        public DateTime? F_LastModifyTime { get; set; }
        public string F_LastModifyUserId { get; set; }
        public DateTime? F_DeleteTime { get; set; }
        public string F_DeleteUserId { get; set; }
    }
}
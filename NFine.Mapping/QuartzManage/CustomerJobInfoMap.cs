//-----------------------------------------------------------------------
// <copyright file=" CustomerJobInfo.cs" company="NFine">
// * Copyright (C) NFine.Framework  All Rights Reserved
// * version : 1.0
// * author  : NFine.Framework
// * FileName: CustomerJobInfo.cs
// * history : Created by T4 07/17/2018 21:29:18 
// </copyright>
//-----------------------------------------------------------------------
using NFine.Domain.Entity.QuartzManage;
using System.Data.Entity.ModelConfiguration;

namespace NFine.Mapping.QuartzManage
{
    public class CustomerJobInfoMap : EntityTypeConfiguration<CustomerJobInfoEntity>
    {
		 public CustomerJobInfoMap()
        {
            this.ToTable("Customer_JobInfo");
            this.HasKey(t => t.F_Id);
        }
    }
}
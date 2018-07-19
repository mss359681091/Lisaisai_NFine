//-----------------------------------------------------------------------
// <copyright file=" CustomerJobInfo.cs" company="NFine">
// * Copyright (C) NFine.Framework  All Rights Reserved
// * version : 1.0
// * author  : NFine.Framework
// * FileName: CustomerJobInfo.cs
// * history : Created by T4 07/17/2018 21:29:19 
// </copyright>
//-----------------------------------------------------------------------
using NFine.Code;
using NFine.Data;
using NFine.Domain.Entity.QuartzManage;
using NFine.Domain.IRepository.QuartzManage;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Repository.QuartzManage
{
    public class CustomerJobInfoRepository : RepositoryBase<CustomerJobInfoEntity>, ICustomerJobInfoRepository
    {
     
    }
}
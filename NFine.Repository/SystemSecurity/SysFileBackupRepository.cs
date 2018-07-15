using NFine.Code;
using NFine.Data;
using NFine.Domain.Entity;
using NFine.Domain.IRepository;
using NFine.Domain.ViewModel;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace NFine.Repository.WebManage
{
    public class SysFileBackupRepository : RepositoryBase<SysFileBackupEntity>, ISysFileBackupRepository
    {
        public List<SysFileBackupDto> GetList(string queryJson, Pagination pagination)
        {
            var queryParam = queryJson.ToJObject();

            StringBuilder strSql = new StringBuilder();
            strSql.Append(@" SELECT TOP " + pagination.rows + " * FROM ( SELECT  ROW_NUMBER() OVER ( ORDER BY d.F_CreatorTime DESC ) AS rownumber ,COUNT(1) OVER() AS records , ");

            strSql.Append(@"  d.*,
                                    i.F_FullName as ServerName 
                            FROM    Sys_FileBackup d
                                    LEFT  JOIN Sys_BackupServer i ON i.F_Id = d.F_ServerIp
                            WHERE   1 = 1
                                    AND (d.F_EnabledMark = 1 or d.F_EnabledMark is null)
                                    AND (d.F_DeleteMark = 0 or d.F_DeleteMark is null)");

            DbParameter[] parameter = new DbParameter[queryParam.Count];

            if (!queryParam["keyword"].IsEmpty())
            {
                strSql.Append(@" AND F_BackupName LIKE  '%'+ @F_BackupName +'%'  ");
                SqlParameter par = new SqlParameter("@F_BackupName", queryParam["keyword"].ToString());
                parameter[0] = par;
            }

            strSql.Append(@" )A  WHERE   rownumber > " + (pagination.page-1) * pagination.rows);
            var result= new RepositoryBase<SysFileBackupDto>().FindList(strSql.ToString(), parameter);
            if (result.Count > 0)
            {
                pagination.records = result[0].records;
            }
            return result;
        }
    }
}
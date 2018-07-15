using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Web.WebApi.Common
{
    public class Users : IUser<string>
    {
        /// <summary>
        /// User构造函数
        /// </summary>
        public Users()
        {

        }
        public string UserId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string TrueName { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// 用户权限
        /// </summary>
        public int Permissions { get; set; }
    }
}

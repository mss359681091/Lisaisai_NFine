using System;
namespace NFine.Domain
{
    public class UserDto
    {
        public string UserId { get; set; }
        public string F_Account { get; set; }
        public string F_RealName { get; set; }
        public string F_NickName { get; set; }
        public string F_HeadIcon { get; set; }
        public bool? F_Gender { get; set; }
        public string F_MobilePhone { get; set; }
        public string F_Email { get; set; }
        public string F_WeChat { get; set; }
        /// <summary>
        /// 岗位名称
        /// </summary>
        public string DutyName { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }
        /// <summary>
        /// 组织名称
        /// </summary>
        public string OrganizeName { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentName { get; set; }

    }
}

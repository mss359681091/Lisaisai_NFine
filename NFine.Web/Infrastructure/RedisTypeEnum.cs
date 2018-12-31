using System.ComponentModel;

namespace NFine.Web.Infrastructure
{
    /// <summary>
    /// Redis存储类型
    /// </summary>
    public enum RedisTypeEnum
    {
        /// <summary>
        /// 日志存储（错误日志）
        /// </summary>
        [Description("错误日志")]
        ExceptionLog = 1,
        /// <summary>
        /// 邮件发送
        /// </summary>
        [Description("邮件发送")]
        Email = 2,
        /// <summary>
        /// 手机短信
        /// </summary>
        [Description("手机短信")]
        SMS = 3,
        /// <summary>
        /// 数据表单
        /// </summary>
        [Description("数据表单")]
        DataForm = 4,
        /// <summary>
        /// 图片转换
        /// </summary>
        [Description("图片转换")]
        IMGConversion = 5,
        /// <summary>
        /// 视频转换
        /// </summary>
        [Description("视频转换")]
        VideoConversion = 6,
        /// <summary>
        /// 日志存储（操作日志）
        /// </summary>
        [Description("操作日志")]
        OperateLog = 7,
        /// <summary>
        /// 日志存储（登录日志）
        /// </summary>
        [Description("登录日志")]
        LoginLog = 8,
    }
}

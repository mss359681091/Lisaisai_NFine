using System.Net.Mail;

namespace Quartz.Net_Infrastructure.MailUtil
{
    public static class SmtpClientConfigurationInfo
    {
        public static string Host { get { return "xxxxx"; } private set { } }
        public static string UserName { get { return "sxxxxx"; } private set { } }
        public static string PassWord { get { return "xxxxx"; } private set { } }
        public static SmtpDeliveryMethod SmtpDeliveryMethod { get{ return SmtpDeliveryMethod.Network; }private set { } }
        public static bool   UseDefaultCredentials { get { return true; }private set { } }
    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace SanJing.Email
{
    /// <summary>
    /// 发送邮箱[System.Net.Mail]
    /// </summary>
    public class Email2
    {
        /// <summary>
        ///  发送带附件邮件（QQ邮箱，SSL，SMTP）（AppSettings[EmailAccount]，AppSettings[EmailPassword]）
        /// </summary>
        /// <param name="sendAccount">发送邮箱地址</param>
        /// <param name="sendPassword">发送邮箱密码</param>
        /// <param name="recieveAccount">收件邮箱地址</param>
        /// <param name="subject">主题</param>
        /// <param name="body">内容（支持HTML）</param>
        /// <param name="filenames">附件</param>
        /// <returns></returns>
        public static bool SendByQQMail(string sendAccount, string sendPassword, string recieveAccount, string subject, string body, params string[] filenames)
        {
            if (string.IsNullOrWhiteSpace(sendAccount))
            {
                throw new ArgumentException("IsNullOrWhiteSpace At AppSettings[EmailAccount]", nameof(sendAccount));
            }

            if (string.IsNullOrWhiteSpace(sendPassword))
            {
                throw new ArgumentException("IsNullOrWhiteSpace At AppSettings[EmailPassword]", nameof(sendPassword));
            }

            if (string.IsNullOrWhiteSpace(recieveAccount))
            {
                throw new ArgumentException("IsNullOrWhiteSpace", nameof(recieveAccount));
            }

            if (string.IsNullOrEmpty(subject))
            {
                throw new ArgumentException("IsNullOrEmpty", nameof(subject));
            }

            if (string.IsNullOrEmpty(body))
            {
                throw new ArgumentException("IsNullOrEmpty", nameof(body));
            }

            MailMessage message = new MailMessage();
            //设置发件人,发件人需要与设置的邮件发送服务器的邮箱一致
            MailAddress fromAddr = new MailAddress(sendAccount);
            message.From = fromAddr;

            //设置收件人,可添加多个,添加方法与下面的一样
            message.To.Add(recieveAccount);

            //设置邮件标题
            message.Subject = subject;

            //设置支持HTML
            message.IsBodyHtml = true;

            //设置邮件内容
            message.Body = body;

            //设置邮件附件
            foreach (var item in filenames)
            {
                message.Attachments.Add(new Attachment(item));
            }

            //设置邮件发送服务器,服务器根据你使用的邮箱而不同,可以到相应的 邮箱管理后台查看,下面是QQ的
            SmtpClient client = new SmtpClient("smtp.qq.com", 587);

            //设置使用自定义凭据
            client.UseDefaultCredentials = false;

            //设置发送人的邮箱账号和密码，POP3/SMTP服务要开启, 密码要是POP3/SMTP等服务的授权码
            client.Credentials = new System.Net.NetworkCredential(sendAccount, sendPassword);

            //启用ssl,也就是安全发送
            client.EnableSsl = true;

            //发送邮件
            client.Send(message);
            return true;
        }
        /// <summary>
        ///  发送带附件邮件（QQ邮箱，SSL，SMTP）（AppSettings[EmailAccount]，AppSettings[EmailPassword]）
        /// </summary>
        /// <param name="recieveAccount">收件邮箱地址</param>
        /// <param name="subject">主题</param>
        /// <param name="body">内容（支持HTML）</param>
        /// <param name="filenames">附件</param>
        /// <returns></returns>
        public static bool SendByQQMail(string recieveAccount, string subject, string body, params string[] filenames)
        {
            string sendAccount = ConfigurationManager.AppSettings["EmailAccount"];
            string sendPassword = ConfigurationManager.AppSettings["EmailPassword"];

            return SendByQQMail(sendAccount, sendPassword, recieveAccount, subject, body, filenames);
        }
        /// <summary>
        ///  发送带附件邮件（QQ企业邮箱，SSL，SMTP）（AppSettings[EmailAccount]，AppSettings[EmailPassword]）
        /// </summary>
        /// <param name="sendAccount">发送邮箱地址</param>
        /// <param name="sendPassword">发送邮箱密码</param>
        /// <param name="recieveAccount">收件邮箱地址</param>
        /// <param name="subject">主题</param>
        /// <param name="body">内容（支持HTML）</param>
        /// <param name="filenames">附件</param>
        /// <returns></returns>
        public static bool SendByQQExMail(string sendAccount, string sendPassword, string recieveAccount, string subject, string body, params string[] filenames)
        {
            if (string.IsNullOrWhiteSpace(sendAccount))
            {
                throw new ArgumentException("IsNullOrWhiteSpace At AppSettings[EmailAccount]", nameof(sendAccount));
            }

            if (string.IsNullOrWhiteSpace(sendPassword))
            {
                throw new ArgumentException("IsNullOrWhiteSpace At AppSettings[EmailPassword]", nameof(sendPassword));
            }

            if (string.IsNullOrWhiteSpace(recieveAccount))
            {
                throw new ArgumentException("IsNullOrWhiteSpace", nameof(recieveAccount));
            }

            if (string.IsNullOrEmpty(subject))
            {
                throw new ArgumentException("IsNullOrEmpty", nameof(subject));
            }

            if (string.IsNullOrEmpty(body))
            {
                throw new ArgumentException("IsNullOrEmpty", nameof(body));
            }

            MailMessage message = new MailMessage();
            //设置发件人,发件人需要与设置的邮件发送服务器的邮箱一致
            MailAddress fromAddr = new MailAddress(sendAccount);
            message.From = fromAddr;

            //设置收件人,可添加多个,添加方法与下面的一样
            message.To.Add(recieveAccount);

            //设置邮件标题
            message.Subject = subject;

            //设置支持HTML
            message.IsBodyHtml = true;

            //设置邮件内容
            message.Body = body;

            //设置邮件附件
            foreach (var item in filenames)
            {
                message.Attachments.Add(new Attachment(item));
            }

            //设置邮件发送服务器,服务器根据你使用的邮箱而不同,可以到相应的 邮箱管理后台查看,下面是QQ的
            SmtpClient client = new SmtpClient("smtp.exmail.qq.com", 587);

            //设置使用自定义凭据
            client.UseDefaultCredentials = false;

            //设置发送人的邮箱账号和密码，POP3/SMTP服务要开启, 密码要是POP3/SMTP等服务的授权码
            client.Credentials = new System.Net.NetworkCredential(sendAccount, sendPassword);

            //启用ssl,也就是安全发送
            client.EnableSsl = true;

            //发送邮件
            client.Send(message);
            return true;
        }
        /// <summary>
        ///  发送带附件邮件（QQ企业邮箱，SSL，SMTP）（AppSettings[EmailAccount]，AppSettings[EmailPassword]）
        /// </summary>
        /// <param name="recieveAccount">收件邮箱地址</param>
        /// <param name="subject">主题</param>
        /// <param name="body">内容（支持HTML）</param>
        /// <param name="filenames">附件</param>
        /// <returns></returns>
        public static bool SendByQQExMail(string recieveAccount, string subject, string body, params string[] filenames)
        {
            string sendAccount = ConfigurationManager.AppSettings["EmailAccount"];
            string sendPassword = ConfigurationManager.AppSettings["EmailPassword"];

            return SendByQQExMail(sendAccount, sendPassword, recieveAccount, subject, body, filenames);
        }
    }
}

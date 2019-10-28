using Limilabs.Mail;
using Limilabs.Mail.Headers;
using Limilabs.Mail.MIME;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace SanJing.Email
{
    /// <summary>
    /// 发送邮箱
    /// </summary>
    public class Email
    {
        /// <summary>
        ///  发送带附件邮件（QQ邮箱，SSL）（AppSettings[EmailAccount]，AppSettings[EmailPassword]）
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

            MailBuilder builder = new MailBuilder();
            builder.From.Add(new MailBox(recieveAccount, recieveAccount));
            builder.To.Add(new MailBox(sendAccount, sendAccount));
            builder.Subject = subject;
            builder.Html = body;
            foreach (var item in filenames)
            {
                MimeData image = builder.AddVisual(item);
                image.ContentId = Path.GetFileName(item);
            }
            IMail email = builder.Create();
            using (Limilabs.Client.SMTP.Smtp smtp = new Limilabs.Client.SMTP.Smtp())
            {
                smtp.ConnectSSL("smtp.qq.com");
                smtp.UseBestLogin(sendAccount, sendPassword);
                var result = smtp.SendMessage(email);
                return result.Status == Limilabs.Client.SMTP.SendMessageStatus.Success;
            }
        }
        /// <summary>
        ///  发送带附件邮件（QQ邮箱，SSL）（AppSettings[EmailAccount]，AppSettings[EmailPassword]）
        /// </summary>
        /// <param name="recieveAccount">收件邮箱地址</param>
        /// <param name="subject">主题</param>
        /// <param name="body">内容（支持HTML）</param>
        /// <param name="filenames">附件</param>
        /// <returns></returns>
        public static bool SendByQQMail(string recieveAccount, string subject, string body, params string[] filenames)
        {
            string sendAccount = ConfigurationManager.AppSettings["EmailAccount"];
            if (string.IsNullOrWhiteSpace(sendAccount))
            {
                throw new ArgumentException("IsNullOrWhiteSpace At AppSettings[EmailAccount]", nameof(sendAccount));
            }

            string sendPassword = ConfigurationManager.AppSettings["EmailPassword"];
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

            MailBuilder builder = new MailBuilder();
            builder.From.Add(new MailBox(recieveAccount, recieveAccount));
            builder.To.Add(new MailBox(sendAccount, sendAccount));
            builder.Subject = subject;
            builder.Html = body;
            foreach (var item in filenames)
            {
                MimeData image = builder.AddVisual(item);
                image.ContentId = Path.GetFileName(item);
            }
            IMail email = builder.Create();
            using (Limilabs.Client.SMTP.Smtp smtp = new Limilabs.Client.SMTP.Smtp())
            {
                smtp.ConnectSSL("smtp.qq.com");
                smtp.UseBestLogin(sendAccount, sendPassword);
                var result = smtp.SendMessage(email);
                return result.Status == Limilabs.Client.SMTP.SendMessageStatus.Success;
            }
        }
        /// <summary>
        ///  发送带附件邮件（QQ企业邮箱，SSL）（AppSettings[EmailAccount]，AppSettings[EmailPassword]）
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

            MailBuilder builder = new MailBuilder();
            builder.From.Add(new MailBox(recieveAccount, recieveAccount));
            builder.To.Add(new MailBox(sendAccount, sendAccount));
            builder.Subject = subject;
            builder.Html = body;
            foreach (var item in filenames)
            {
                MimeData image = builder.AddVisual(item);
                image.ContentId = Path.GetFileName(item);
            }
            IMail email = builder.Create();
            using (Limilabs.Client.SMTP.Smtp smtp = new Limilabs.Client.SMTP.Smtp())
            {
                smtp.ConnectSSL("smtp.exmail.qq.com");
                smtp.UseBestLogin(sendAccount, sendPassword);
                var result = smtp.SendMessage(email);
                return result.Status == Limilabs.Client.SMTP.SendMessageStatus.Success;
            }
        }
        /// <summary>
        ///  发送带附件邮件（QQ企业邮箱，SSL）（AppSettings[EmailAccount]，AppSettings[EmailPassword]）
        /// </summary>
        /// <param name="recieveAccount">收件邮箱地址</param>
        /// <param name="subject">主题</param>
        /// <param name="body">内容（支持HTML）</param>
        /// <param name="filenames">附件</param>
        /// <returns></returns>
        public static bool SendByQQExMail(string recieveAccount, string subject, string body, params string[] filenames)
        {
            string sendAccount = ConfigurationManager.AppSettings["EmailAccount"];
            if (string.IsNullOrWhiteSpace(sendAccount))
            {
                throw new ArgumentException("IsNullOrWhiteSpace At AppSettings[EmailAccount]", nameof(sendAccount));
            }

            string sendPassword = ConfigurationManager.AppSettings["EmailPassword"];
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

            MailBuilder builder = new MailBuilder();
            builder.From.Add(new MailBox(recieveAccount, recieveAccount));
            builder.To.Add(new MailBox(sendAccount, sendAccount));
            builder.Subject = subject;
            builder.Html = body;
            foreach (var item in filenames)
            {
                MimeData image = builder.AddVisual(item);
                image.ContentId = Path.GetFileName(item);
            }
            IMail email = builder.Create();
            using (Limilabs.Client.SMTP.Smtp smtp = new Limilabs.Client.SMTP.Smtp())
            {
                smtp.ConnectSSL("smtp.exmail.qq.com");
                smtp.UseBestLogin(sendAccount, sendPassword);
                var result = smtp.SendMessage(email);
                return result.Status == Limilabs.Client.SMTP.SendMessageStatus.Success;
            }
        }
    }
}

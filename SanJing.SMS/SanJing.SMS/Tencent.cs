using qcloudsms_csharp;
using qcloudsms_csharp.httpclient;
using qcloudsms_csharp.json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SanJing.SMS
{
    /// <summary>
    /// 腾讯云
    /// </summary>
    public class Tencent
    {
        /// <summary>
        /// 指定模板 ID 单发短信(86)(默认签名)
        /// </summary>
        /// <param name="phoneNumber">手机号</param>
        /// <param name="values">替换值|按顺序替换</param>
        /// <param name="templateId">模板Id</param>
        /// <param name="appid">APPID</param>
        /// <param name="appkey">APPKEY</param>
        public static void Send(int templateId, string[] values, string phoneNumber, int appid, string appkey)
        {
            SmsSingleSender ssender = new SmsSingleSender(appid, appkey);
            var result = ssender.sendWithParam("86", phoneNumber, templateId, values, "", "", "");
            if (result.result != 0)
                throw new Exception(new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(result));
        }
        /// <summary>
        /// 指定模板 ID 单发短信(86)(默认签名)
        /// </summary>
        /// <param name="templateId">模板Id</param>
        /// <param name="values">替换值|按顺序替换</param>
        /// <param name="phoneNumbers">手机号</param>
        /// <param name="appid">APPID</param>
        /// <param name="appkey">APPKEY</param>
        public static void Send(int templateId, string[] values, string[] phoneNumbers, int appid, string appkey)
        {
            SmsMultiSender msender = new SmsMultiSender(appid, appkey);
            var result = msender.sendWithParam("86", phoneNumbers, templateId, values, "", "", "");
            if (result.result != 0)
                throw new Exception(new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(result));
        }
        /// <summary>
        /// 发送语音通知
        /// 数字默认按照个十百千万进行播报
        /// </summary>
        /// <param name="voiceContent">语音文本|示例：您的语音验证码是5,6,7,8</param>
        /// <param name="phoneNumber">手机号</param>
        /// <param name="appid">APPID</param>
        /// <param name="appkey">APPKEY</param>
        public static void SendVoice(string voiceContent, string phoneNumber, int appid, string appkey)
        {
            SmsVoicePromptSender vspsender = new SmsVoicePromptSender(appid, appkey);
            var result = vspsender.send("86", phoneNumber, 2, voiceContent, 2, "");
            if (result.result != 0)
                throw new Exception(new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(result));
        }
    }
}

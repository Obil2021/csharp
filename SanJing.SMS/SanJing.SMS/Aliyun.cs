using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Dysmsapi.Model.V20170525;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SanJing.SMS
{
    /// <summary>
    /// 阿里云
    /// </summary>
    public class Aliyun
    {
        const string REGIONIDFORPOP = "cn-hangzhou";
        const string PRODUCT = "Dysmsapi";
        const string DOMAIN = "dysmsapi.aliyuncs.com";
        const string OK = "OK";
        /// <summary>
        /// 群发短信
        /// </summary>
        /// <param name="templateCode">模板ID|管理控制台中配置的审核通过的短信模板的模板CODE（状态必须是验证通过）</param>
        /// <param name="templateParam">模板中的变量</param>
        /// <param name="phoneNumbers">手机号</param>
        /// <param name="signName">签名|管理控制台中配置的短信签名（状态必须是验证通过）</param>
        /// <param name="accessId">APPID</param>
        /// <param name="accessSecret">APPKEY</param>
        public static void Send(string templateCode, Dictionary<string, string> templateParam, string[] phoneNumbers, string signName, string accessId, string accessSecret)
        {
            IClientProfile profile = DefaultProfile.GetProfile(REGIONIDFORPOP, accessId, accessSecret);
            DefaultProfile.AddEndpoint(REGIONIDFORPOP, REGIONIDFORPOP, PRODUCT, DOMAIN);
            IAcsClient acsClient = new DefaultAcsClient(profile);
            SendSmsRequest request = new SendSmsRequest();
            request.PhoneNumbers = string.Join(",", phoneNumbers);
            request.SignName = signName;
            request.TemplateCode = templateCode;
            request.TemplateParam = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(templateParam);
            SendSmsResponse sendSmsResponse = acsClient.GetAcsResponse(request);
            if (sendSmsResponse.Code != OK)
                throw new Exception(new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(sendSmsResponse));
        }
        /// <summary>
        /// 单发短信
        /// </summary>
        /// <param name="templateCode">模板ID|管理控制台中配置的审核通过的短信模板的模板CODE（状态必须是验证通过）</param>
        /// <param name="templateParam">模板中的变量</param>
        /// <param name="phoneNumber">手机号</param>
        /// <param name="signName">签名|管理控制台中配置的短信签名（状态必须是验证通过）</param>
        /// <param name="accessId">APPID</param>
        /// <param name="accessSecret">APPKEY</param>
        public static void Send(string templateCode, Dictionary<string, string> templateParam, string phoneNumber, string signName, string accessId, string accessSecret)
        {
            Send(templateCode, templateParam, new[] { phoneNumber }, signName, accessId, accessSecret);
        }
    }
}

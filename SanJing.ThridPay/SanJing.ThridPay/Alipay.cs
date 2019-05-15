using Aop.Api;
using Aop.Api.Domain;
using Aop.Api.Request;
using Aop.Api.Response;
using Aop.Api.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace SanJing.ThridPay
{
    public class Alipay
    {
        private const string BEGIN_RSA_PRIVATE_KEY = "-----BEGIN RSA PRIVATE KEY-----";
        private const string END_RSA_PRIVATE_KEY = "-----END RSA PRIVATE KEY-----";
        /// <summary>
        /// 支付回调(始终返回成功)
        /// </summary>
        /// <param name="form">支付宝POST过来的数据</param>
        /// <param name="alipay_public_key">支付宝公钥(开放平台可查)</param>
        /// <param name="action">执行操作</param>
        /// <param name="exception">异常处理</param>
        /// <returns></returns>
        public static string PayCallBack(NameValueCollection form, string alipay_public_key, Action<string, double> action, Action<Exception> exception)
        {
            var formData = form.AllKeys.ToDictionary(key => key, key => form[key]);
            if (AlipaySignature.RSACheckV1(formData, alipay_public_key, "utf-8", "RSA2", false))
            {
                var ordernumber = formData["out_trade_no"];
                var amount = double.Parse(formData["total_amount"]);
                try
                {
                    action?.Invoke(ordernumber, amount);
                }
                catch (Exception ex)
                {
                    exception?.Invoke(ex);
                }
            }
            else
                exception?.Invoke(new Exception("验签失败：" + JsonConvert.SerializeObject(formData)));
            return "success";
        }
        /// <summary>
        /// 支付（APP）
        /// </summary>
        /// <param name="ordernumber">订单编号</param>
        /// <param name="amount">订单金额（元）</param>
        /// <param name="subject">公司名称</param>
        /// <param name="body">项目名称</param>
        /// <param name="callbackurl">回调完整地址</param>
        /// <param name="appid">APPID</param>
        /// <param name="app_private_key_path">应用私钥文件完整路径|rsa_private_key.pem</param>
        /// <param name="alipay_public_key">支付宝公钥(开放平台可查)</param>
        /// <returns></returns>
        public static string PayByAPP(string ordernumber, double amount, string subject, string body, string callbackurl, string appid, string app_private_key_path, string alipay_public_key)
        {
            var app_private_key = getAppPrivateKeyFromFile(app_private_key_path);
            var alipayClient = new DefaultAopClient("https://openapi.alipay.com/gateway.do", appid, app_private_key, "JSON", "1.0", "RSA2", alipay_public_key, "utf-8", false);
            AlipayTradeAppPayModel apppay = new AlipayTradeAppPayModel();
            apppay.Body = body;
            apppay.OutTradeNo = ordernumber;
            apppay.ProductCode = "QUICK_MSECURITY_PAY";
            apppay.Subject = subject;
            apppay.TimeoutExpress = "30m";
            apppay.TotalAmount = amount.ToString("f2");
            AlipayTradeAppPayRequest request = new AlipayTradeAppPayRequest();
            request.SetBizModel(apppay);
            request.SetNotifyUrl(callbackurl);
            AlipayTradeAppPayResponse response = alipayClient.SdkExecute(request);
            if (response.IsError)
                throw new Exception(response.Body);
            return response.Body;
        }
        /// <summary>
        /// 支付(H5)
        /// </summary>
        /// <param name="ordernumber">订单编号</param>
        /// <param name="amount">订单金额（元）</param>
        /// <param name="subject">公司名称</param>
        /// <param name="body">项目名称</param>
        /// <param name="callbackurl">回调完整地址</param>
        /// <param name="appid">APPID</param>
        /// <param name="app_private_key_path">应用私钥文件完整路径|rsa_private_key.pem</param>
        /// <param name="alipay_public_key">支付宝公钥(开放平台可查)</param>
        /// <returns></returns>
        public static string PayByH5(string ordernumber, double amount, string subject, string body, string callbackurl, string appid, string app_private_key_path, string alipay_public_key)
        {
            var app_private_key = getAppPrivateKeyFromFile(app_private_key_path);
            var alipayClient = new DefaultAopClient("https://openapi.alipay.com/gateway.do", appid, app_private_key, "JSON", "1.0", "RSA2", alipay_public_key, "utf-8", true);
            AlipayTradePagePayModel apppay = new AlipayTradePagePayModel();

            apppay.Body = body;
            apppay.OutTradeNo = ordernumber;
            apppay.ProductCode = "FAST_INSTANT_TRADE_PAY";
            apppay.Subject = subject;
            apppay.TimeoutExpress = "30m";
            apppay.TotalAmount = amount.ToString("f2");
            AlipayTradePagePayRequest request = new AlipayTradePagePayRequest();
            request.SetBizModel(apppay);
            request.SetNotifyUrl(callbackurl);
            AlipayTradePagePayResponse response = alipayClient.pageExecute(request, null, "GET");
            if (response.IsError)
                throw new Exception(response.Body);
            return response.Body;
        }
        /// <summary>
        /// 退款
        /// </summary>
        /// <param name="ordernumber">退款订单编号</param>
        /// <param name="pay_ordernumber">支付订单编号</param>
        /// <param name="refund_amount">退款金额（元）</param>
        /// <param name="appid">APPID</param>
        /// <param name="app_private_key_path">应用私钥文件完整路径|rsa_private_key.pem</param>
        /// <param name="alipay_public_key">支付宝公钥(开放平台可查)</param>
        public static void Refund(string ordernumber, string pay_ordernumber, double refund_amount, string appid, string app_private_key_path, string alipay_public_key)
        {
            var app_private_key = getAppPrivateKeyFromFile(app_private_key_path);
            var alipayClient = new DefaultAopClient("https://openapi.alipay.com/gateway.do", appid, app_private_key, "JSON", "1.0", "RSA2", alipay_public_key, "utf-8", false);
            AlipayTradeRefundRequest request = new AlipayTradeRefundRequest();
            request.BizContent = JsonConvert.SerializeObject(new
            {
                out_request_no = ordernumber,
                out_trade_no = pay_ordernumber,
                refund_amount = refund_amount.ToString("f2")
            });
            AlipayTradeRefundResponse response = alipayClient.Execute(request);
            if (response.IsError)
                throw new Exception(response.Body);
        }
        /// <summary>
        /// 转账到个人支付宝账户
        /// </summary>
        /// <param name="ordernumber">订单编号</param>
        /// <param name="amount">转账金额（元）</param>
        /// <param name="account">支付宝账号</param>
        /// <param name="username">用户真实姓名</param>
        /// <param name="subject">公司名称</param>
        /// <param name="body">项目名称</param>
        /// <param name="appid">APPID</param>
        /// <param name="app_private_key_path">应用私钥文件完整路径|rsa_private_key.pem</param>
        /// <param name="alipay_public_key">支付宝公钥(开放平台可查)</param>
        public static void Transfer(string ordernumber, double amount, string account, string username, string subject, string body, string appid, string app_private_key_path, string alipay_public_key)
        {
            var app_private_key = getAppPrivateKeyFromFile(app_private_key_path);
            var alipayClient = new DefaultAopClient("https://openapi.alipay.com/gateway.do", appid, app_private_key, "JSON", "1.0", "RSA2", alipay_public_key, "utf-8", false);
            AlipayFundTransToaccountTransferRequest request = new AlipayFundTransToaccountTransferRequest();
            request.BizContent = JsonConvert.SerializeObject(new
            {
                out_biz_no = ordernumber,
                payee_type = "ALIPAY_LOGONID",
                payee_account = account,
                amount = amount.ToString("f2"),
                payer_show_name = subject,
                payee_real_name = username,
                remark = body
            });
            AlipayFundTransToaccountTransferResponse response = alipayClient.Execute(request);
            if (response.IsError)
                throw new Exception(response.Body);
        }
        private static string getAppPrivateKeyFromFile(string app_private_key_path)
        {
            var app_private_key = System.IO.File.ReadAllText(app_private_key_path);
            app_private_key = app_private_key.Replace(BEGIN_RSA_PRIVATE_KEY, string.Empty)
                .Replace(END_RSA_PRIVATE_KEY, string.Empty).Trim();
            return app_private_key;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SanJing.ThridPay
{
    /// <summary>
    /// 微信支付
    /// </summary>
    public class Wxpay
    {
        private const string URL_ORDER_QUERY = "https://api.mch.weixin.qq.com/pay/orderquery";
        private const string URL_ORDER_MICRO = "https://api.mch.weixin.qq.com/pay/micropay";
        private const string URL_ORDER_PAY = "https://api.mch.weixin.qq.com/pay/unifiedorder";
        private const string URL_ORDER_REFUND = "https://api.mch.weixin.qq.com/secapi/pay/refund";
        private const string URL_ORDER_TRANSFER = "https://api.mch.weixin.qq.com/mmpaymkttransfers/promotion/transfers";
        private const string URL_REDPACK_SEND = "https://api.mch.weixin.qq.com/mmpaymkttransfers/sendredpack";
        private const string RUL_REDPACK_GROUP_SEND = "https://api.mch.weixin.qq.com/mmpaymkttransfers/sendgroupredpack";
        private const string MSG_ISNULLORWHITESPACE = "IsNullOrWhiteSpace";
        /// <summary>
        /// 统一下单查询（支持所有平台的下单）
        /// https://pay.weixin.qq.com/wiki/doc/api/micropay.php?chapter=9_2
        /// </summary>
        /// <param name="orderNumber">订单编号</param>
        /// <param name="appId">APPID</param>
        /// <param name="mchId">商户ID</param>
        /// <param name="mchKey">商户密钥</param>
        /// <returns>订单信息</returns>
        public static IDictionary<string, object> PayQuery(string orderNumber, string appId, string mchId, string mchKey)
        {
            if (string.IsNullOrWhiteSpace(orderNumber))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(orderNumber));
            }
            if (string.IsNullOrWhiteSpace(appId))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(appId));
            }

            if (string.IsNullOrWhiteSpace(mchId))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(mchId));
            }

            if (string.IsNullOrWhiteSpace(mchKey))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(mchKey));
            }

            var order = new Dictionary<string, object>();
            order.Add("appid", appId);
            order.Add("mch_id", mchId);
            order.Add("nonce_str", WxBase.WxBase.NonceStr());
            order.Add("out_trade_no", orderNumber);
            order.Add("sign", WxBase.WxBase.Md5Sign(mchKey, order));
            var result = WxBase.WxBase.ApiPostXmlRequest(order, URL_ORDER_QUERY, true);
            return result;
        }

        /// <summary>
        /// 统一下单（付款码版）
        /// https://pay.weixin.qq.com/wiki/doc/api/micropay.php?chapter=9_10&index=1
        /// </summary>
        /// <param name="amount">金额</param>
        /// <param name="body">描述</param>
        /// <param name="orderNumber">订单编号</param>
        /// <param name="auth_code">扫码支付授权码，设备读取用户微信中的条码或者二维码信息（注：用户付款码条形码规则：18位纯数字，以10、11、12、13、14、15开头）</param>
        /// <param name="appId">APPID</param>
        /// <param name="mchId">商户ID</param>
        /// <param name="mchKey">商户密钥</param>
        /// <param name="ipAddress">服务器（当前）IP地址</param>
        /// <returns></returns>
        public static IDictionary<string, object> PayByMicro(double amount, string body, string orderNumber,
            string auth_code, string appId, string mchId, string mchKey, string ipAddress)
        {
            if (string.IsNullOrWhiteSpace(body))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(body));
            }

            if (string.IsNullOrWhiteSpace(orderNumber))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(orderNumber));
            }

            if (string.IsNullOrWhiteSpace(auth_code))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(auth_code));
            }

            if (string.IsNullOrWhiteSpace(appId))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(appId));
            }

            if (string.IsNullOrWhiteSpace(mchId))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(mchId));
            }

            if (string.IsNullOrWhiteSpace(mchKey))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(mchKey));
            }

            if (string.IsNullOrWhiteSpace(ipAddress))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(ipAddress));
            }
            var order = new Dictionary<string, object>();
            order.Add("appid", appId);
            order.Add("mch_id", mchId);
            order.Add("nonce_str", WxBase.WxBase.NonceStr());
            order.Add("body", body);
            order.Add("out_trade_no", orderNumber);
            order.Add("total_fee", (amount * 100).ToString("f0"));
            order.Add("spbill_create_ip", ipAddress);
            order.Add("auth_code", auth_code);
            order.Add("sign", WxBase.WxBase.Md5Sign(mchKey, order));
            var result = WxBase.WxBase.ApiPostXmlRequest(order, URL_ORDER_MICRO, true);
            return result;
        }
        /// <summary>
        /// 统一下单（移动端H5版）
        /// https://pay.weixin.qq.com/wiki/doc/api/H5.php?chapter=9_20&index=1
        /// </summary>
        /// <param name="amount">金额</param>
        /// <param name="body">描述</param>
        /// <param name="orderNumber">商户订单号</param>
        /// <param name="callbackUrl">回调地址</param>
        /// <param name="appId">APPID(公众号)</param>
        /// <param name="mchId">商户ID</param>
        /// <param name="mchKey">商户密钥</param>
        /// <param name="ipAddress">服务器（当前）IP地址</param>
        /// <returns>移动端网页调起支付需要跳转的URL地址</returns>
        public static string PayByH5(double amount, string body, string orderNumber,
            string callbackUrl, string appId, string mchId, string mchKey, string ipAddress)
        {
            if (string.IsNullOrWhiteSpace(body))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(body));
            }

            if (string.IsNullOrWhiteSpace(orderNumber))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(orderNumber));
            }

            if (string.IsNullOrWhiteSpace(callbackUrl))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(callbackUrl));
            }

            if (string.IsNullOrWhiteSpace(appId))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(appId));
            }

            if (string.IsNullOrWhiteSpace(mchId))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(mchId));
            }

            if (string.IsNullOrWhiteSpace(mchKey))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(mchKey));
            }

            if (string.IsNullOrWhiteSpace(ipAddress))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(ipAddress));
            }

            var order = new Dictionary<string, object>();
            order.Add("appid", appId);
            order.Add("mch_id", mchId);
            order.Add("nonce_str", WxBase.WxBase.NonceStr());
            order.Add("body", body);
            order.Add("out_trade_no", orderNumber);
            order.Add("total_fee", (amount * 100).ToString("f0"));
            order.Add("spbill_create_ip", ipAddress);
            order.Add("notify_url", callbackUrl);
            order.Add("trade_type", "MWEB");
            order.Add("sign", WxBase.WxBase.Md5Sign(mchKey, order));
            var result = WxBase.WxBase.ApiPostXmlRequest(order, URL_ORDER_PAY, true);
            return result["mweb_url"].ToString();
        }
        /// <summary>
        /// 统一下单（移动应用版）
        /// https://pay.weixin.qq.com/wiki/doc/api/app/app.php?chapter=9_1
        /// </summary>
        /// <param name="amount">金额</param>
        /// <param name="body">描述</param>
        /// <param name="orderNumber">商户订单号</param>
        /// <param name="callbackUrl">回调地址</param>
        /// <param name="appId">APPID（开放平台）</param>
        /// <param name="mchId">商户ID</param>
        /// <param name="mchKey">商户密钥</param>
        /// <param name="ipAddress">服务器（当前）IP地址</param>
        /// <returns>移动端调起支付需要的参数</returns>
        public static IDictionary<string, object> PayByAPP(double amount, string body, string orderNumber,
            string callbackUrl, string appId, string mchId, string mchKey, string ipAddress)
        {
            if (string.IsNullOrWhiteSpace(body))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(body));
            }

            if (string.IsNullOrWhiteSpace(orderNumber))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(orderNumber));
            }

            if (string.IsNullOrWhiteSpace(callbackUrl))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(callbackUrl));
            }

            if (string.IsNullOrWhiteSpace(appId))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(appId));
            }

            if (string.IsNullOrWhiteSpace(mchId))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(mchId));
            }

            if (string.IsNullOrWhiteSpace(mchKey))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(mchKey));
            }

            if (string.IsNullOrWhiteSpace(ipAddress))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(ipAddress));
            }

            var order = new Dictionary<string, object>();
            order.Add("appid", appId);
            order.Add("mch_id", mchId);
            order.Add("nonce_str", WxBase.WxBase.NonceStr());
            order.Add("body", body);
            order.Add("out_trade_no", orderNumber);
            order.Add("total_fee", (amount * 100).ToString("f0"));
            order.Add("spbill_create_ip", ipAddress);
            order.Add("notify_url", callbackUrl);
            order.Add("trade_type", "APP");
            order.Add("sign", WxBase.WxBase.Md5Sign(mchKey, order));
            var result = WxBase.WxBase.ApiPostXmlRequest(order, URL_ORDER_PAY, true);
            var data = new Dictionary<string, object>()  {
                            { "appid",appId},
                            { "partnerid",mchId},
                            { "timestamp",WxBase.WxBase.TimeStamp()},
                            { "noncestr",WxBase.WxBase.NonceStr()},
                            { "package","Sign=WXPay"},
                            { "prepayid",result["prepay_id"].ToString() }
                        };
            data.Add("sign", WxBase.WxBase.Md5Sign(mchKey, data));
            return data;
        }
        /// <summary>
        /// 统一下单（微信内网页和小程序版）
        /// 微信内：https://pay.weixin.qq.com/wiki/doc/api/jsapi.php?chapter=9_1
        /// 小程序：https://pay.weixin.qq.com/wiki/doc/api/wxa/wxa_api.php?chapter=9_1
        /// </summary>
        /// <param name="amount">金额</param>
        /// <param name="body">描述</param>
        /// <param name="orderNumber">商户订单编号</param>
        /// <param name="callbackUrl">回调完整地址</param>
        /// <param name="appId">APPID</param>
        /// <param name="mchId">商户ID（公众号|小程序）</param>
        /// <param name="mchKey">商户密钥</param>
        /// <param name="ipAddress">服务器（当前）IP地址</param>
        /// <param name="openId">微信用户编号</param>
        /// <returns>小程序或微信内网页端调起支付需要的参数</returns>
        public static IDictionary<string, object> PayByWx(double amount, string body, string orderNumber,
            string callbackUrl, string appId, string mchId, string mchKey, string ipAddress, string openId)
        {
            if (string.IsNullOrWhiteSpace(body))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(body));
            }

            if (string.IsNullOrWhiteSpace(orderNumber))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(orderNumber));
            }

            if (string.IsNullOrWhiteSpace(callbackUrl))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(callbackUrl));
            }

            if (string.IsNullOrWhiteSpace(appId))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(appId));
            }

            if (string.IsNullOrWhiteSpace(mchId))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(mchId));
            }

            if (string.IsNullOrWhiteSpace(mchKey))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(mchKey));
            }

            if (string.IsNullOrWhiteSpace(ipAddress))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(ipAddress));
            }

            if (string.IsNullOrWhiteSpace(openId))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(openId));
            }

            var order = new Dictionary<string, object>();
            order.Add("appid", appId);
            order.Add("mch_id", mchId);
            order.Add("nonce_str", WxBase.WxBase.NonceStr());
            order.Add("body", body);
            order.Add("openid", openId);
            order.Add("out_trade_no", orderNumber);
            order.Add("total_fee", (amount * 100).ToString("f0"));
            order.Add("spbill_create_ip", ipAddress);
            order.Add("notify_url", callbackUrl);
            order.Add("trade_type", "JSAPI");
            order.Add("sign", WxBase.WxBase.Md5Sign(mchKey, order));
            var result = WxBase.WxBase.ApiPostXmlRequest(order, URL_ORDER_PAY, true);
            var data = new Dictionary<string, object>()  {
                            { "appId",appId},
                            { "timeStamp", WxBase.WxBase.TimeStamp()},
                            { "nonceStr",WxBase.WxBase.NonceStr()},
                            { "signType","MD5"},
                            { "package","prepay_id="+result["prepay_id"].ToString()}
                        };
            data.Add("paySign", WxBase.WxBase.Md5Sign(mchKey, data));
            return data;
        }
        /// <summary>
        /// 统一下单（扫一扫）（展示二维码供微信扫码支付）
        /// https://pay.weixin.qq.com/wiki/doc/api/native.php?chapter=9_1
        /// </summary>
        /// <param name="amount">金额</param>
        /// <param name="body">描述</param>
        /// <param name="orderNumber">商户订单编号</param>
        /// <param name="callbackUrl">回调完整地址</param>
        /// <param name="appId">APPID（公众号|小程序）</param>
        /// <param name="mchId">商户ID</param>
        /// <param name="mchKey">商户密钥</param>
        /// <param name="ipAddress">服务器（当前）IP地址</param>
        /// <returns>生成二维码需要的字符串</returns>
        public static string PayByQR(double amount, string body, string orderNumber, string callbackUrl,
            string appId, string mchId, string mchKey, string ipAddress)
        {
            if (string.IsNullOrWhiteSpace(body))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(body));
            }

            if (string.IsNullOrWhiteSpace(orderNumber))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(orderNumber));
            }

            if (string.IsNullOrWhiteSpace(callbackUrl))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(callbackUrl));
            }

            if (string.IsNullOrWhiteSpace(appId))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(appId));
            }

            if (string.IsNullOrWhiteSpace(mchId))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(mchId));
            }

            if (string.IsNullOrWhiteSpace(mchKey))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(mchKey));
            }

            if (string.IsNullOrWhiteSpace(ipAddress))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(ipAddress));
            }

            var order = new Dictionary<string, object>();
            order.Add("appid", appId);
            order.Add("mch_id", mchId);
            order.Add("nonce_str", WxBase.WxBase.NonceStr());
            order.Add("body", body);
            order.Add("out_trade_no", orderNumber);
            order.Add("total_fee", (amount * 100).ToString("f0"));
            order.Add("spbill_create_ip", ipAddress);
            order.Add("notify_url", callbackUrl);
            order.Add("trade_type", "NATIVE");
            order.Add("sign", WxBase.WxBase.Md5Sign(mchKey, order));
            var result = WxBase.WxBase.ApiPostXmlRequest(order, URL_ORDER_PAY, true);
            return result["code_url"].ToString();
        }
        /// <summary>
        /// 统一下单 回调处理（注意微信回调会出现多次回调的情况，所以数据库处理之前请一定要判断是否已经处理过，已经处理过的请跳过。）
        /// https://pay.weixin.qq.com/wiki/doc/api/jsapi.php?chapter=9_16&index=10
        /// </summary>
        /// <param name="inputStream">微信发送过来的流</param>
        /// <param name="mchKey">商户密钥（验签）</param>
        /// <param name="action">回调处理方法，参数为商户订单编号和金额</param>
        /// <param name="exception">异常处理</param>
        /// <returns>将此结果返回给微信（始终为SUCCESS）</returns>
        public static string PayCallBack(Stream inputStream, string mchKey, Action<string, double> action,
            Action<Exception> exception)
        {
            if (inputStream == null)
            {
                throw new ArgumentNullException(nameof(inputStream));
            }

            if (string.IsNullOrWhiteSpace(mchKey))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(mchKey));
            }

            try
            {
                var formData = WxBase.WxBase.StreamToDictionary(inputStream, mchKey);
                var ordernumber = formData["out_trade_no"].ToString();
                var amount = Convert.ToDouble(formData["total_fee"]) / 100;
                action?.Invoke(ordernumber, amount);
            }
            catch (Exception ex)
            {
                exception?.Invoke(ex);
            }
            return WxBase.WxBase.SuccessString;
        }
        /// <summary>
        /// 统一退款 （支持开放平台，公众号，小程序订单）
        /// https://pay.weixin.qq.com/wiki/doc/api/native.php?chapter=9_4
        /// </summary>
        /// <param name="refundOrderNumber">退款单号</param>
        /// <param name="refundAmount">退款金额（元）</param>
        /// <param name="orderNumber">付款单号</param>
        /// <param name="amount">付款金额</param>
        /// <param name="appId">APPID</param>
        /// <param name="mchId">商户号</param>
        /// <param name="mchKey">商户密钥</param>
        /// <param name="certFileNAME">商户密钥完整文件地址</param>
        /// <returns></returns>
        public static IDictionary<string, object> Refund(string refundOrderNumber, double refundAmount,
            string orderNumber, double amount, string appId, string mchId, string mchKey, string certFileNAME)
        {
            if (string.IsNullOrWhiteSpace(refundOrderNumber))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(refundOrderNumber));
            }

            if (string.IsNullOrWhiteSpace(orderNumber))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(orderNumber));
            }

            if (string.IsNullOrWhiteSpace(appId))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(appId));
            }

            if (string.IsNullOrWhiteSpace(mchId))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(mchId));
            }

            if (string.IsNullOrWhiteSpace(mchKey))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(mchKey));
            }

            if (string.IsNullOrWhiteSpace(certFileNAME))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(certFileNAME));
            }

            var data = new Dictionary<string, object>();
            data.Add("appid", appId);
            data.Add("mch_id", mchId);
            data.Add("nonce_str", WxBase.WxBase.NonceStr());
            data.Add("out_trade_no", orderNumber);
            data.Add("out_refund_no", refundOrderNumber);
            data.Add("total_fee", (amount * 100).ToString("f0"));
            data.Add("refund_fee", (refundAmount * 100).ToString("f0"));
            data.Add("sign", WxBase.WxBase.Md5Sign(mchKey, data));
            return WxBase.WxBase.ApiPostXmlRequestWithCert(data, mchId, certFileNAME,
                URL_ORDER_REFUND, true);
        }
        /// <summary>
        /// 企业付款（支持开放平台，公众号，小程序订单）
        /// https://pay.weixin.qq.com/wiki/doc/api/tools/mch_pay.php?chapter=14_2
        /// </summary>
        /// <param name="orderNumber">付款编号</param>
        /// <param name="amount">付款金额（元）</param>
        /// <param name="openId">OPENID</param>
        /// <param name="desc">描述，用户可见</param>
        /// <param name="appId">APPIS</param>
        /// <param name="mchId">商户号</param>
        /// <param name="mchkey">商户密钥</param>
        /// <param name="ipAddress">服务器（当前）IP地址</param>
        /// <param name="certFileName">商户密钥文件完整路径</param>
        /// <returns></returns>
        public static IDictionary<string, object> Transfer(string orderNumber, double amount, string openId,
            string desc, string appId, string mchId, string mchkey, string ipAddress, string certFileName)
        {
            if (string.IsNullOrWhiteSpace(orderNumber))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(orderNumber));
            }

            if (string.IsNullOrWhiteSpace(openId))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(openId));
            }

            if (string.IsNullOrWhiteSpace(desc))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(desc));
            }

            if (string.IsNullOrWhiteSpace(appId))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(appId));
            }

            if (string.IsNullOrWhiteSpace(mchId))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(mchId));
            }

            if (string.IsNullOrWhiteSpace(mchkey))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(mchkey));
            }

            if (string.IsNullOrWhiteSpace(ipAddress))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(ipAddress));
            }

            if (string.IsNullOrWhiteSpace(certFileName))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(certFileName));
            }

            var data = new Dictionary<string, object>();
            data.Add("mch_appid", appId);
            data.Add("mchid", mchId);
            data.Add("nonce_str", WxBase.WxBase.NonceStr());
            data.Add("partner_trade_no", orderNumber);
            data.Add("openid", openId);
            data.Add("check_name", "NO_CHECK");
            data.Add("amount", (amount * 100).ToString("f0"));
            data.Add("desc", desc);
            data.Add("spbill_create_ip", ipAddress);
            data.Add("sign", WxBase.WxBase.Md5Sign(mchkey, data));
            return WxBase.WxBase.ApiPostXmlRequestWithCert(data, mchId, certFileName,
                URL_ORDER_TRANSFER, true);
        }
        /// <summary>
        /// 企业发红包（仅支持公众号或企业号）
        /// 普通红包：https://pay.weixin.qq.com/wiki/doc/api/tools/cash_coupon.php?chapter=13_4&index=3
        /// 裂变红包：https://pay.weixin.qq.com/wiki/doc/api/tools/cash_coupon.php?chapter=13_5&index=4
        /// </summary>
        /// <param name="orderNumber">红包编号</param>
        /// <param name="amount">红包金额</param>
        /// <param name="openId">OPENID|种子用户，多个红包时，它可以分享给别人领取红包</param>
        /// <param name="sendName">商户名称</param>
        /// <param name="appId">APPID|企业号corpid即为此appid</param>
        /// <param name="mchId">商户号</param>
        /// <param name="mchKey">商户密钥</param>
        /// <param name="ipAddress">服务器（当前）IP地址</param>
        /// <param name="certFileName">商户密钥文件完整路径</param>
        /// <param name="wishing">祝福语</param>
        /// <param name="actName">活动名称</param>
        /// <param name="remark">备注</param>
        /// <param name="totalNum">红包个数，默认值：1</param>
        /// <returns></returns>
        public static IDictionary<string, object> RedPack(string orderNumber, double amount, string openId,
            string sendName, string appId, string mchId, string mchKey, string ipAddress, string certFileName,
            string wishing, string actName, string remark, int totalNum = 1)
        {
            if (string.IsNullOrWhiteSpace(orderNumber))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(orderNumber));
            }

            if (string.IsNullOrWhiteSpace(openId))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(openId));
            }

            if (string.IsNullOrWhiteSpace(sendName))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(sendName));
            }

            if (string.IsNullOrWhiteSpace(appId))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(appId));
            }

            if (string.IsNullOrWhiteSpace(mchId))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(mchId));
            }

            if (string.IsNullOrWhiteSpace(mchKey))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(mchKey));
            }

            if (string.IsNullOrWhiteSpace(ipAddress))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(ipAddress));
            }

            if (string.IsNullOrWhiteSpace(certFileName))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(certFileName));
            }

            if (string.IsNullOrWhiteSpace(wishing))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(wishing));
            }

            if (string.IsNullOrWhiteSpace(actName))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(actName));
            }

            if (string.IsNullOrWhiteSpace(remark))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(remark));
            }

            var data = new Dictionary<string, object>();
            if (totalNum > 1)
                data.Add("amt_type", "ALL_RAND");
            else
                data.Add("client_ip", ipAddress);
            data.Add("wxappid", appId);
            data.Add("mchid", mchId);
            data.Add("nonce_str", WxBase.WxBase.NonceStr());
            data.Add("mch_billno", orderNumber);
            data.Add("re_openid", openId);
            data.Add("total_num", totalNum);
            data.Add("total_amount", (amount * 100).ToString("f0"));
            data.Add("send_name", sendName);
            data.Add("wishing", wishing);
            data.Add("act_name", actName);
            data.Add("remark", remark);
            data.Add("sign", WxBase.WxBase.Md5Sign(mchKey, data));
            return WxBase.WxBase.ApiPostXmlRequestWithCert(data, mchId, certFileName,
                totalNum == 1 ? URL_REDPACK_SEND :
                RUL_REDPACK_GROUP_SEND, true);
        }

    }
}

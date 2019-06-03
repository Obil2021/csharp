using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace SanJing.WebApi
{
    /// <summary>
    /// 接口基础类
    /// </summary>
    public class WebApiController : ApiController
    {
        /// <summary>
        /// 某些浏览器跨域请求时会先请求此方法
        /// </summary>
        /// <returns></returns>
        [HttpOptions]
        [SwaggerHide]
        public HttpResponseMessage Options()
        {
            return Request.CreateResponse(System.Net.HttpStatusCode.Accepted);
        }
    }
    /// <summary>
    /// 接口基础类(默认APPKEY:447AD97D40C592FBDA322EC14794132A)
    /// </summary>
    /// <typeparam name="RequestT">请求模型</typeparam>
    /// <typeparam name="ResponseT">返回模型</typeparam>
    public abstract class WebApiController<RequestT, ResponseT> : WebApiController
        where RequestT : RequestModel where ResponseT : ResponseModel, new()
    {
        /// <summary>
        /// 请重写此方法
        /// </summary>
        /// <param name="request"></param>
        /// <returns>ResponseT</returns>
        [HttpPost]
        public virtual ResponseT Post(RequestT request)
        {
            this.RequestReady(request, WebApiConfig.AppKey);
            return new ResponseT();
        }
    }
    /// <summary>
    /// 接口基础类(自定义APPKEY)
    /// </summary>
    /// <typeparam name="RequestT">请求模型</typeparam>
    /// <typeparam name="ResponseT">返回模型</typeparam>
    /// <typeparam name="AppKeyT">APPKEY</typeparam>
    public abstract class WebApiController<RequestT, ResponseT, AppKeyT> : WebApiController<RequestT, ResponseT>
       where RequestT : RequestModel where ResponseT : ResponseModel, new() where AppKeyT : WebApiAppKey, new()
    {
        /// <summary>
        /// 请重写此方法
        /// </summary>
        /// <param name="request"></param>
        /// <returns>ResponseT</returns>
        [HttpPost]
        public override ResponseT Post(RequestT request)
        {
            this.RequestReady(request, new AppKeyT().AppKey);
            return new ResponseT();
        }
    }
}

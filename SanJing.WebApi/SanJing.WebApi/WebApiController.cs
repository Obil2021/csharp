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
        public HttpResponseMessage OPTIONS()
        {
            return Request.CreateResponse(System.Net.HttpStatusCode.Accepted);
        }
    }
}

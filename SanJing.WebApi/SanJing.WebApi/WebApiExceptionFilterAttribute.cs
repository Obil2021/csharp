using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http.Filters;

namespace SanJing.WebApi
{
    /// <summary>
    /// WebApi全局异常处理
    /// 2000+ 表示用户可见
    /// 3000+ 表示程序员可见
    /// </summary>
    public class WebApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// 异常处理
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            //日志记录
            NLog.LogManager.GetCurrentClassLogger().Error(actionExecutedContext.Exception);

            if (actionExecutedContext.Exception is TokenException)
            {
                actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(System.Net.HttpStatusCode.OK, new ResponseModel()
                {
                    StatuCode = 2000,
                    StatuMsg = actionExecutedContext.Exception.Message
                });
            }
            else if (actionExecutedContext.Exception is UserException)
            {
                actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(System.Net.HttpStatusCode.OK, new ResponseModel()
                {
                    StatuCode = 2001,
                    StatuMsg = actionExecutedContext.Exception.Message
                });
            }
            else if (actionExecutedContext.Exception is ValidException)
            {
                actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(System.Net.HttpStatusCode.OK, new ResponseModel()
                {
                    StatuCode = 3000,
                    StatuMsg = actionExecutedContext.Exception.Message
                });
            }
            else if (actionExecutedContext.Exception is SignException)
            {
                actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(System.Net.HttpStatusCode.OK, new ResponseModel()
                {
                    StatuCode = 3001,
                    StatuMsg = actionExecutedContext.Exception.Message
                });
            }
            else if (actionExecutedContext.Exception is IPAddressException)
            {
                actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(System.Net.HttpStatusCode.OK, new ResponseModel()
                {
                    StatuCode = 3002,
                    StatuMsg = actionExecutedContext.Exception.Message
                });
            }
            else if (actionExecutedContext.Exception is TimeStampException)
            {
                actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(System.Net.HttpStatusCode.OK, new ResponseModel()
                {
                    StatuCode = 3003,
                    StatuMsg = actionExecutedContext.Exception.Message
                });
            }
            else if (actionExecutedContext.Exception is NonceStringException)
            {
                actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(System.Net.HttpStatusCode.OK, new ResponseModel()
                {
                    StatuCode = 3004,
                    StatuMsg = actionExecutedContext.Exception.Message
                });
            }
            else
            {
                actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(System.Net.HttpStatusCode.OK, new ResponseModel()
                {
                    StatuCode = 4000,
                    StatuMsg = "系统繁忙"
                });
            }
        }
    }
}

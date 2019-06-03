using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http.Description;

namespace SanJing.WebApi
{
    /// <summary>
    /// API说明
    /// </summary>
    public class Swagger
    {
        /// <summary>
        /// XML文件路径
        /// </summary>
        /// <param name="name">XML文件名（一般与项目名称相同）</param>
        /// <returns></returns>
        public static string GetXmlCommentsPath(string name)
        {
            return $"{AppDomain.CurrentDomain.BaseDirectory}\\bin\\{name}.xml";
        }
        /// <summary>
        /// 根据命名空间（文件夹）分组[Replace("Controllers", string.Empty) + " APIs"]
        /// </summary>
        /// <param name="apiDescription"></param>
        /// <returns></returns>
        public static string GroupActionsBy(ApiDescription apiDescription)
        {
            string result = apiDescription.ActionDescriptor.ControllerDescriptor.ControllerType.Namespace.Split('.').Last();
            result = result.Replace("Controllers", string.Empty) + " APIs";
            return result == " APIs" ? "General" + result : result;
        }
        /// <summary>
        /// SwaggerAPI路由模型[api/General/{{controller}}/{{id}}]
        /// </summary>
        /// <returns></returns>
        public static string RouteTemplate()
        {
            return $"api/General/{{controller}}/{{id}}";
        }
        /// <summary>
        /// SwaggerAPI路由模型[api/{webApiAppKey.AppName}/{{controller}}/{{id}}]
        /// </summary>
        /// <param name="webApiAppKey"></param>
        /// <returns></returns>
        public static string RouteTemplate(WebApiAppKey webApiAppKey)
        {
            return $"api/{webApiAppKey.AppName}/{{controller}}/{{id}}";
        }
    }
    /// <summary>
    /// 隐藏接口
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public partial class SwaggerHideAttribute : Attribute { }

    /// <summary>
    /// 过滤隐藏接口【SwaggerHide】
    /// </summary>
    public class ApplyDocumentVendorExtensions : IDocumentFilter
    {
        /// <summary>
        /// Apply
        /// </summary>
        /// <param name="swaggerDoc"></param>
        /// <param name="schemaRegistry"></param>
        /// <param name="apiExplorer"></param>
        public virtual void Apply(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, IApiExplorer apiExplorer)
        {
            foreach (ApiDescription apiDescription in apiExplorer.ApiDescriptions)
            {
                IEnumerable<SwaggerHideAttribute> swaggerHideAttributes = apiDescription.GetControllerAndActionAttributes<SwaggerHideAttribute>();
                if (swaggerHideAttributes.Any())
                {
                    string key = "/" + apiDescription.RelativePathSansQueryString();
                    if (apiDescription.HttpMethod == HttpMethod.Options)
                        swaggerDoc.paths[key].options = null;
                    else if (apiDescription.HttpMethod == HttpMethod.Post)
                        swaggerDoc.paths[key].post = null;
                    else if (apiDescription.HttpMethod == HttpMethod.Get)
                        swaggerDoc.paths[key].get = null;
                    else if (apiDescription.HttpMethod == HttpMethod.Delete)
                        swaggerDoc.paths[key].delete = null;
                    else if (apiDescription.HttpMethod == HttpMethod.Head)
                        swaggerDoc.paths[key].head = null;
                    else if (apiDescription.HttpMethod == HttpMethod.Put)
                        swaggerDoc.paths[key].put = null;
                    else
                        swaggerDoc.paths[key].patch = null;
                }
            }

            foreach (ApiDescription apiDescription in apiExplorer.ApiDescriptions)
            {
                string groupname = Swagger.GroupActionsBy(apiDescription);
                if (!groupname.StartsWith(apiDescription.RelativePathSansQueryString().Split('/')[1]))
                {
                    string key = "/" + apiDescription.RelativePathSansQueryString();
                    swaggerDoc.paths.Remove(key);
                }
            }
        }
    }
}

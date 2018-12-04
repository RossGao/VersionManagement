using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text;

namespace VersionManagement.Utils
{
    /// <summary>
    /// Use ETage for response caching.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ETagFilterAttribute : Attribute, IActionFilter
    {
        private readonly int[] _statusCodes;

        public ETagFilterAttribute(params int[] statusCodes)
        {
            _statusCodes = statusCodes;
            if (statusCodes.Length == 0) _statusCodes = new[] { 200 };
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.HttpContext.Request.Method == "GET")
            {
                if (_statusCodes.Contains(context.HttpContext.Response.StatusCode))
                {
                    //I just serialize the result to JSON, could do something less costly
                    var content = JsonConvert.SerializeObject(context.Result);

                    var etag = ETagGenerator.GetETag(context.HttpContext.Request.Path.ToString(), Encoding.UTF8.GetBytes(content));

                    if (context.HttpContext.Request.Headers.Keys.Contains("If-None-Match") && context.HttpContext.Request.Headers["If-None-Match"].ToString() == etag)
                    {
                        context.Result = new StatusCodeResult(304);
                    }

                    context.HttpContext.Response.Headers.Add("ETag", new[] { etag });
                }
            }
        }

        public int[] StatusCodes { get; }
    }
}

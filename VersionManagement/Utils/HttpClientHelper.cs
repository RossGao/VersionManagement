using Newtonsoft.Json;
using NLog;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace VersionManagement.Utils
{
    public static class HttpClientHelper
    {
        /// <summary>
        /// 发送http get请求从另一个服务地址获取信息
        /// </summary>
        /// <param name="client">HttpClient实例</param>
        /// <param name="reqPath">请求资源相对路径</param>
        /// <returns>Get请求结果</returns>
        public static async Task<T> GetAsync<T>(HttpClient client, string reqPath)
        {
            if (client != null)
            {
                client.DefaultRequestHeaders.Accept.Clear();
                var resp = await client.GetAsync(reqPath);

                if (resp == null || resp.IsSuccessStatusCode == false)
                {
                    var errMsg = resp == null ? "返回为空" : await resp.Content.ReadAsStringAsync();
                    LogHelper.LogMessage(LogLevel.Warn, $"url:{reqPath}. Error:{errMsg}. Code:{resp.StatusCode}");
                    return default(T);
                }

                var jsonContent = await resp.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(jsonContent);
            }

            return default(T);
        }

        /// <summary>
        /// 发送post请求到另一个服务
        /// </summary>
        /// <typeparam name="T">Post body对象类型</typeparam>
        /// <typeparam name="R">Return 对象类型</typeparam>
        /// <param name="client">HttpClient实例</param>
        /// <param name="reqPath">资源相对路径</param>
        /// <param name="data">post对象</param>
        /// <returns>Post请求结果</returns>
        public static async Task<R> PostAsync<T, R>(HttpClient client, string reqPath, T data)
        {
            if (client != null)
            {
                client.DefaultRequestHeaders.Accept.Clear();
                var resp = await client.PostAsync(
                    reqPath,
                    new StringContent(JsonConvert.SerializeObject(data),
                    Encoding.UTF8,
                    "application/json"));

                if (resp == null || resp.IsSuccessStatusCode == false)
                {
                    var errMsg = resp == null ? "返回为空" : resp.Content.ReadAsStringAsync().Result;
                    LogHelper.LogMessage(LogLevel.Warn, $"url:{reqPath}. Error:{errMsg}. Code:{resp.StatusCode}");
                    return default(R);
                }

                var jsonContent = resp.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<R>(jsonContent);
            }

            return default(R);
        }

        /// <summary>
        /// 设置服务访问重试机制，对于网络问题或服务端问题，以及返回404的情况使用重试机制，已2的幂指数倍时间间隔进行重新请求，
        /// 同时加上jitter时间差避免所有客户端同时进行重试造成的负担。
        /// </summary>
        /// <returns>针对返回结果的重试策略</returns>
        public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            Random jitterer = new Random();

            return HttpPolicyExtensions
                .HandleTransientHttpError()
                //.OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                + +TimeSpan.FromMilliseconds(jitterer.Next(0, 100)));
        }

        /// <summary>
        /// 设置服务访问超时机制，对于get请求超过10秒为超时，其它请求超过30秒为超时
        /// </summary>
        /// <param name="req">Http请求</param>
        /// <returns>针对返回结果的超时策略</returns>
        public static TimeoutPolicy<HttpResponseMessage> GetTimeoutPolicy(HttpRequestMessage req)
        {
            TimeoutPolicy<HttpResponseMessage> timeout;

            if (req.Method == HttpMethod.Get)
            {
                timeout = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(60));
            }
            else
            {
                timeout = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(90));
            }

            return timeout;
        }

        /// <summary>
        /// 设置循环中断模式客户端访问政策。当网络错误或者服务端错误出现5次时, 在30秒内无法再次请求服务，所有请求会立即返回失败，30秒后请求可以继续。
        /// </summary>
        /// <returns>针对返回结果的中断请求策略</returns>
        public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
        }
    }
}

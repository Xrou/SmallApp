using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesktopSolution.Miscellaneous
{
    class HttpClient
    {
        ///<summary>
        ///get/post/put/delete/... some data from url, includes auth header and returns response
        ///</summary>
        public static ShortHttpResponse Send(string url, Method method, KeyValuePair<string, object>[]? parameters = null, Dictionary<string, object>? body = null)
        {
            var options = new RestClientOptions($"https://localhost:{ConnectionSettings.Port}")
            {
                RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
            };
            var client = new RestClient(options);

            var request = new RestRequest(url, method);

            request.AddHeader("Content-Type", "application/json");

            if (body != null)
            {
                string stringBody = JsonSerializer.Serialize(body);
                request.AddJsonBody(stringBody);
            }

            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    request.AddParameter(parameter.Key, parameter.Value?.ToString());
                }
            }

            var response = client.ExecuteAsync(request).Result;

            return new ShortHttpResponse(response.StatusCode, response.Content);
        }

        public record struct ShortHttpResponse(HttpStatusCode code, string? response);
    }
}
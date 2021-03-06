﻿using IdentityModel.HttpSigning.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
#if PORTABLE
using Flurl;
#else
using System.Net.Http.Formatting;
#endif
using System.Threading.Tasks;

namespace IdentityModel.HttpSigning
{
    public class RequestSigningOptions
    {
#if !LIBLOG_PORTABLE
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();
#else
        private static readonly ILog Logger = LogProvider.GetLogger(nameof(RequestSigningOptions));
#endif

        public bool SignMethod { get; set; }
        public bool SignHost { get; set; }
        public bool SignPath { get; set; }
        public bool SignAllQueryParameters { get; set; }
        public IEnumerable<string> QueryParametersToSign { get; set; }
        public IEnumerable<string> RequestHeadersToSign { get; set; }
        public bool SignBody { get; set; }

        public async Task<EncodingParameters> CreateEncodingParametersAsync(HttpRequestMessage request)
        {
            if (request == null) throw new ArgumentNullException("request");

            var token = request.GetAccessToken();
            if (token == null) return null;

            var parameters = new EncodingParameters(token);

            if (SignMethod)
            {
                Logger.Debug("Encoding method");
                parameters.Method = request.Method;
            }

            if (SignHost)
            {
                Logger.Debug("Encoding host");
                parameters.Host = request.RequestUri.Host;
            }

            if (SignPath)
            {
                Logger.Debug("Encoding path");
                parameters.Path = request.RequestUri.AbsolutePath;
            }

            var queryParamsToSign = GetQueryParamsToSign(request.RequestUri);
            foreach (var item in queryParamsToSign)
            {
                Logger.DebugFormat("Encoding query string param: {0}", item.Key);
                parameters.QueryParameters.Add(item);
            }

            var headersToSign = GetRequestHeadersToSign(request);
            foreach(var item in headersToSign)
            {
                Logger.DebugFormat("Encoding request header: {0}", item.Key);
                parameters.RequestHeaders.Add(item);
            }

            if (SignBody)
            {
                Logger.Debug("Encoding body");
                parameters.Body = await request.ReadBodyAsync();
            }

            return parameters;
        }

        private IEnumerable<KeyValuePair<string, string>> GetQueryParamsToSign(Uri url)
        {
            if (!SignAllQueryParameters && 
                (QueryParametersToSign == null || !QueryParametersToSign.Any()))
            {
                return Enumerable.Empty<KeyValuePair<string, string>>();
            }

#if PORTABLE
            var queryParams = Url.ParseQueryParams(url.Query.ToString());
            IEnumerable<KeyValuePair<string, string>> query = queryParams.Select(qp => new KeyValuePair<string, string>(qp.Name, (string)qp.Value));
#else
            IEnumerable<KeyValuePair<string, string>> query = new FormDataCollection(url);
#endif
            if (SignAllQueryParameters == false)
            {
                query = query.Where(x => QueryParametersToSign.Contains(x.Key));
            }

            return query.OrderBy(x => x.Value, StringComparer.OrdinalIgnoreCase);
        }

        private IEnumerable<KeyValuePair<string, string>> GetRequestHeadersToSign(HttpRequestMessage request)
        {
            if (RequestHeadersToSign == null || !RequestHeadersToSign.Any())
            {
                return Enumerable.Empty<KeyValuePair<string, string>>();
            }

            var list =
                from h in request.Headers
                from v in h.Value
                where RequestHeadersToSign.Contains(h.Key)
                select new KeyValuePair<string, string>(h.Key, v);

            return list.OrderBy(x => x.Value, StringComparer.OrdinalIgnoreCase);
        }
    }
}

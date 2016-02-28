﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;

namespace IdentityModel.HttpSigning
{
    public class EncodingParameters
    {
        public EncodingParameters(string accessToken)
        {
            if (String.IsNullOrWhiteSpace(accessToken))
            {
                throw new ArgumentNullException("accessToken");
            }

            AccessToken = accessToken;
            TimeStamp = DateTimeOffset.UtcNow;
            QueryParameters = new List<KeyValuePair<string, string>>();
            RequestHeaders = new List<KeyValuePair<string, string>>();
        }

        public string AccessToken { get; private set; }
        public DateTimeOffset TimeStamp { get; set; }
        public HttpMethod HttpMethod { get; set; }
        public string Host { get; set; }
        public string UrlPath { get; set; }
        public IList<KeyValuePair<string, string>> QueryParameters { get; set; }
        public IList<KeyValuePair<string, string>> RequestHeaders { get; set; }
        public byte[] Body { get; set; }

        public Dictionary<string, object> ToEncodedDictionary()
        {
            var value = new Dictionary<string, object>();

            value.Add(HttpSigningConstants.SignedObjectParameterNames.AccessToken, AccessToken);
            value.Add(HttpSigningConstants.SignedObjectParameterNames.TimeStamp, TimeStamp.ToEpochTime());

            if (HttpMethod != null)
            {
                value.Add(HttpSigningConstants.SignedObjectParameterNames.HttpMethod, HttpMethod.Method);
            }

            if (Host != null)
            {
                value.Add(HttpSigningConstants.SignedObjectParameterNames.Host, Host);
            }

            if (UrlPath != null)
            {
                value.Add(HttpSigningConstants.SignedObjectParameterNames.UrlPath, UrlPath);
            }

            if (QueryParameters != null && QueryParameters.Any())
            {
                var query = new EncodingQueryParameters(QueryParameters);
                var array = query.ToEncodedArray();
                value.Add(HttpSigningConstants.SignedObjectParameterNames.HashedQueryParameters, array);
            }

            if (RequestHeaders != null && RequestHeaders.Any())
            {
                var headers = new EncodingHeaderList(RequestHeaders);
                var array = headers.ToEncodedArray();
                value.Add(HttpSigningConstants.SignedObjectParameterNames.HashedRequestHeaders, array);
            }

            if (Body != null)
            {
                value.Add(HttpSigningConstants.SignedObjectParameterNames.HashedRequestBody, CalculateBodyHash());
            }

            return value;
        }

        string CalculateBodyHash()
        {
            var hash = SHA256.Create().ComputeHash(Body);
            return Jose.Base64Url.Encode(hash);
        }

        public bool IsSame(EncodingParameters other)
        {
            return false;
        }
    }
}

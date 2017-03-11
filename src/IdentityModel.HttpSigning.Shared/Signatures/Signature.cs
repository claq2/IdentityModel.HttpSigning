// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel.HttpSigning.Logging;
#if PORTABLE
using JosePCL;
#else
using Jose;
using System.Security.Cryptography;
#endif
using System;

namespace IdentityModel.HttpSigning
{
    public abstract class Signature
    {
#if !LIBLOG_PORTABLE
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();
#else
        private static readonly ILog Logger = LogProvider.GetLogger(nameof(Signature));
#endif

        private readonly JwsAlgorithm _alg;
        private readonly object _key;

        protected Signature(JwsAlgorithm alg, object key = null)
        {
            _alg = alg;
            _key = key;
        }

        public abstract string Alg { get; }

        public string Sign(EncodingParameters payload)
        {
            if (payload == null) throw new ArgumentNullException("payload");

            var encodedPayload = payload.Encode();
            return JWT.Encode(encodedPayload.Encode(), _key, _alg);
        }

        public EncodedParameters Verify(string token)
        {
            if (token == null) throw new ArgumentNullException("token");

            try
            {
                var headers = JWT.Headers(token);
                if (headers == null || !headers.ContainsKey(HttpSigningConstants.Jwk.AlgorithmProperty))
                {
                    Logger.Error("Token does not contain " + HttpSigningConstants.Jwk.AlgorithmProperty + " property in header");
                    return null;
                }

                var alg = headers[HttpSigningConstants.Jwk.AlgorithmProperty];
                if (!Alg.Equals(alg))
                {
                    Logger.Error("Signature alg does not match token alg");
                    return null;
                }

                var json = JWT.Decode(token, _key);
                if (json == null)
                {
                    Logger.Error("Failed to decode token");
                    return null;
                }

                return EncodedParameters.FromJson(json);
            }
            catch(Exception ex)
            {
                Logger.ErrorException("Failed to decode token", ex);
            }

            return null;
        }
    }
}

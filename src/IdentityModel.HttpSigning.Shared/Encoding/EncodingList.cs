﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel.HttpSigning.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
#if PORTABLE
using PCLCrypto;
#else
using System.Security.Cryptography;
#endif
using System.Text;

namespace IdentityModel.HttpSigning
{
    public class EncodingList
    {
#if !LIBLOG_PORTABLE
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();
#else
        private static readonly ILog Logger = LogProvider.GetLogger(nameof(EncodingList));
#endif

        public EncodingList(
            ICollection<KeyValuePair<string, string>> list,
            string keyValueSeparator,
            string parameterSeparator, 
            bool lowerCaseKeys)
        {
            if (list == null) throw new ArgumentNullException("list");
            if (list.Any() == false) throw new ArgumentException("list is empty");
            if (keyValueSeparator == null) throw new ArgumentNullException("keyValueSeparator");
            if (parameterSeparator == null) throw new ArgumentNullException("parameterSeparator");

            Encode(list, keyValueSeparator, parameterSeparator, lowerCaseKeys);
        }

        void Encode(
            ICollection<KeyValuePair<string, string>> list, 
            string keyValueSeparator, 
            string parameterSeparator,
            bool lowerCaseKeys)
        {
            var keys = new List<string>();
            var values = new StringBuilder();

            foreach (var item in list)
            {
                var key = item.Key;
                if (lowerCaseKeys)
                {
                    key = key.ToLowerInvariant();
                }
                var value = item.Value;

                keys.Add(key);
                if (values.Length > 0)
                {
                    values.Append(parameterSeparator);
                }

                values.Append(key);
                values.Append(keyValueSeparator);
                values.Append(value);

                Logger.DebugFormat("Encoding key: {0}", key);
            }

            Keys = keys;
            Value = values.ToString();
        }

        public IEnumerable<string> Keys { get; private set; }
        public string Value { get; private set; }

        public EncodedList Encode()
        {
            var bytes = Encoding.UTF8.GetBytes(Value);
#if PORTABLE
            var hasher = WinRTCrypto.HashAlgorithmProvider.OpenAlgorithm(HashAlgorithm.Sha256);
            var hash = hasher.HashData(bytes);
#else
            var hash = SHA256.Create().ComputeHash(bytes);
#endif
            var value = Base64Url.Encode(hash);

            return new EncodedList(Keys, value);
        }
    }
}

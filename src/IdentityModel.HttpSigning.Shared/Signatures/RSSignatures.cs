// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
#if PORTABLE
using PCLCrypto;
#else
using System.Security.Cryptography;
#endif

namespace IdentityModel.HttpSigning
{
    static class RSAParametersExtensions
    {
#if PORTABLE
        public static ICryptographicKey ToRSACryptoServiceProvider(this RSAParameters rsa)
        {
            IAsymmetricKeyAlgorithmProvider provider = WinRTCrypto.AsymmetricKeyAlgorithmProvider.OpenAlgorithm(AsymmetricAlgorithm.RsaPkcs1);
            var key = provider.ImportParameters(rsa);
            return key;
        }
#else
        public static RSACryptoServiceProvider ToRSACryptoServiceProvider(this RSAParameters rsa)
        {
            var csp = new CspParameters();
            csp.Flags = CspProviderFlags.CreateEphemeralKey;
            csp.KeyNumber = (int)KeyNumber.Signature;

            var prov = new RSACryptoServiceProvider(2048, csp);
            prov.ImportParameters(rsa);

            return prov;
        }
#endif
    }

    public class RS256Signature : Signature
    {
        public RS256Signature(RSAParameters rsa)
            : base(JwsAlgorithm.RS256, rsa.ToRSACryptoServiceProvider())
        {
        }

#if PORTABLE
        public RS256Signature(ICryptographicKey key)
#else
        public RS256Signature(RSACryptoServiceProvider key)
#endif
            : base(JwsAlgorithm.RS256, key)
        {
        }

        public override string Alg { get { return "RS256"; } }
    }

    public class RS384Signature : Signature
    {
        public RS384Signature(RSAParameters rsa)
            : base(JwsAlgorithm.RS384, rsa.ToRSACryptoServiceProvider())
        {
        }

#if PORTABLE
        public RS384Signature(ICryptographicKey key)
#else
        public RS384Signature(RSACryptoServiceProvider key)
#endif
            : base(JwsAlgorithm.RS384, key)
        {
        }

        public override string Alg { get { return "RS384"; } }
    }

    public class RS512Signature : Signature
    {
        public RS512Signature(RSAParameters rsa)
            : base(JwsAlgorithm.RS512, rsa.ToRSACryptoServiceProvider())
        {
        }

#if PORTABLE
        public RS512Signature(ICryptographicKey key)
#else
        public RS512Signature(RSACryptoServiceProvider key)
#endif
            : base(JwsAlgorithm.RS512, key)
        {
        }

        public override string Alg { get { return "RS512"; }}
    }
}

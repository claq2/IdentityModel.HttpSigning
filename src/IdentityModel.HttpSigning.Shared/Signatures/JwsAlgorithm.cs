using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityModel.HttpSigning
{
    internal class JwsAlgorithmMapper
    {
#if PORTABLE
         public static Dictionary<JwsAlgorithm, string> JwsAlgorithmMap = new Dictionary<JwsAlgorithm, string>
        {
            { JwsAlgorithm.ES256, JosePCL.JwsAlgorithms.ES256 },
            { JwsAlgorithm.ES384, JosePCL.JwsAlgorithms.ES384 },
            { JwsAlgorithm.ES512, JosePCL.JwsAlgorithms.ES512 },
            { JwsAlgorithm.HS256, JosePCL.JwsAlgorithms.HS256 },
            { JwsAlgorithm.HS384, JosePCL.JwsAlgorithms.HS384 },
            { JwsAlgorithm.HS512, JosePCL.JwsAlgorithms.HS512 },
            { JwsAlgorithm.none, JosePCL.JwsAlgorithms.None },
            { JwsAlgorithm.PS256, JosePCL.JwsAlgorithms.PS256 },
            { JwsAlgorithm.PS384, JosePCL.JwsAlgorithms.PS384 },
            { JwsAlgorithm.PS512, JosePCL.JwsAlgorithms.PS512 },
            { JwsAlgorithm.RS256, JosePCL.JwsAlgorithms.RS256 },
            { JwsAlgorithm.RS384, JosePCL.JwsAlgorithms.RS384 },
            { JwsAlgorithm.RS512, JosePCL.JwsAlgorithms.RS512 },
        };
#else
        public static Dictionary<JwsAlgorithm, Jose.JwsAlgorithm> JwsAlgorithmMap = new Dictionary<JwsAlgorithm, Jose.JwsAlgorithm>
        {
            { JwsAlgorithm.ES256, Jose.JwsAlgorithm.ES256 },
            { JwsAlgorithm.ES384, Jose.JwsAlgorithm.ES384 },
            { JwsAlgorithm.ES512, Jose.JwsAlgorithm.ES512 },
            { JwsAlgorithm.HS256, Jose.JwsAlgorithm.HS256 },
            { JwsAlgorithm.HS384, Jose.JwsAlgorithm.HS384 },
            { JwsAlgorithm.HS512, Jose.JwsAlgorithm.HS512 },
            { JwsAlgorithm.none, Jose.JwsAlgorithm.none },
            { JwsAlgorithm.PS256, Jose.JwsAlgorithm.PS256 },
            { JwsAlgorithm.PS384, Jose.JwsAlgorithm.PS384 },
            { JwsAlgorithm.PS512, Jose.JwsAlgorithm.PS512 },
            { JwsAlgorithm.RS256, Jose.JwsAlgorithm.RS256 },
            { JwsAlgorithm.RS384, Jose.JwsAlgorithm.RS384 },
            { JwsAlgorithm.RS512, Jose.JwsAlgorithm.RS512 },
        };
#endif
    }

    public enum JwsAlgorithm
    {
        none = 0,
        HS256 = 1,
        HS384 = 2,
        HS512 = 3,
        RS256 = 4,
        RS384 = 5,
        RS512 = 6,
        PS256 = 7,
        PS384 = 8,
        PS512 = 9,
        ES256 = 10,
        ES384 = 11,
        ES512 = 12
    }
}

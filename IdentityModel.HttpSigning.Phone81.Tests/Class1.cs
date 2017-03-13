using Flurl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityModel.HttpSigning.Phone81.Tests
{
    public class Class1
    {
        public void Test()
        {
            var url = new Uri("http://blah.com?a=1&b=2");
            var q = Url.ParseQueryParams(url.ToString());
            IEnumerable<KeyValuePair<string, string>> t = q.Select(qi => new KeyValuePair<string, string>(qi.Name, (string)qi.Value));
        }

    }
}

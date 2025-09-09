using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace UElements.Helpers
{
    public static class ParamsExtensions
    {
        public static Dictionary<string, string> FromParams(this IEnumerable<Param> values)
        {
            return values.ToDictionary(a => a.Key, a => a.Value);
        }

        public static Dictionary<string, string> FromQuery(this string query)
        {
            NameValueCollection dict = HttpUtility.ParseQueryString(query);
            return dict.Cast<string>().ToDictionary(k => k, v => dict[v]);
        }
    }
}
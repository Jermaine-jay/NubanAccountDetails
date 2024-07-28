using System.Web;

namespace NubanAccountDetails.Paystack
{
    public static class Extension
    {
        public static string ToQueryString(this object request)
        {
            IEnumerable<string> source = from p in request.GetType().GetProperties()
                                         let v = p.GetValue(request, null)
                                         where v != null
                                         select p.Name + "=" + HttpUtility.UrlEncode(v.ToString());
            return string.Join("&", source.ToArray());
        }
    }
}

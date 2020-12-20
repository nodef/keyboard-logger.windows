using System.Text;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using Map = System.Collections.Generic.Dictionary<string, string>;


namespace KeyLog.Common {
  class Http {
    
    // Perform an HTTP request.
    public static async Task<string> Request(string method, string url, Map headers, string type, string data) {
      var bytes = Encoding.UTF8.GetBytes(data);
      // setup request
      var r = (HttpWebRequest) WebRequest.Create(url);
      r.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
      r.Method = method;
      r.ContentType = type;
      r.ContentLength = bytes.Length;
      foreach (var h in headers)
        r.Headers.Add(h.Key, h.Value);
      // write body
      using (var body = r.GetRequestStream())
        await body.WriteAsync(bytes, 0, bytes.Length);
      // get response
      using (var s = (HttpWebResponse) await r.GetResponseAsync())
      using (var body = s.GetResponseStream())
      using (var bodyr = new StreamReader(body))
        return await bodyr.ReadToEndAsync();
    }
  }
}

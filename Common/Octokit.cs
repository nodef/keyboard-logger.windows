using System.Threading.Tasks;
using System.Collections.Generic;


namespace KeyLog.Common {
  class Octokit {

    Dictionary<string, string> Headers;
    public string Owner;
    public string Repo;


    public Octokit(OctokitConfig c) : this(c.Token) {
      Owner = c.Owner;
      Repo  = c.Repo;
    }

    public Octokit(string token) {
      Headers = new Dictionary<string, string>() {
        ["Accept"] = "application/vnd.github.v3+json",
        ["Authorization"] = "token "+token,
        ["Content-Type"] = "application/json"
      };
    }


    public async Task<string> CreateIssue(string title, string body) {
      string url = string.Format("https://api.github.com/repos/{0}/{1}/issues", Owner, Repo);
      return await Http.Request("POST", url, Headers,
        string.Format("{{\"owner\": \"{0}\", \"repo\": \"{1}\", \"title\": \"{2}\", \"body\": \"{3}\"}}",
        Owner, Repo, title, body));
    }
  }


  struct OctokitConfig {
    public string Token;
    public string Owner;
    public string Repo;

    public OctokitConfig(Dictionary<string, string> options, string prefix="") {
      var o = options;
      var p = prefix;
      o.TryGetValue(p+"token", out Token);
      o.TryGetValue(p+"owner", out Owner);
      o.TryGetValue(p+"repo",  out Repo);
    }
  }
}

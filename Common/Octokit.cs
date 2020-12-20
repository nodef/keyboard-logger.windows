using System.Threading.Tasks;
using Map = System.Collections.Generic.Dictionary<string, string>;


namespace KeyLog.Common {
  class Octokit {

    readonly string Owner;
    readonly string Repo;
    readonly Map Headers;

    public Octokit(Map o) {
      Owner = o["owner"];
      Repo = o["repo"];
      Headers = new Map() {
        ["Authorization"] = "token "+o["token"],
        ["Accept"] = "application/vnd.github.v3+json"
      };
    }

    public async Task<string> CreateIssue(string title, string body) {
      string url = string.Format("https://api.github.com/repos/{0}/{0}/issues", Owner, Repo);
      return await Http.Request("POST", url, Headers, "application/json",
        string.Format("{\"owner\": \"{0}\", \"repo\": \"{0}\", \"title\": \"{0}\", \"body\": \"{0}\"}",
        Owner, Repo, title, body));
    }
  }
}

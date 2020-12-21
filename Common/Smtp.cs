using System;
using System.Net;
using System.Net.Mail;
using System.Collections.Generic;


namespace KeyLog.Common {
  class Smtp {

    SmtpClient Client;
    public string From;
    public string To;


    public Smtp(SmtpConfig c) : this(c.Host, c.Port, c.Username, c.Password) {
      From = c.From;
      To   = c.To;
    }

    public Smtp(string host, int port, string username="", string password="") {
      Client = new SmtpClient(host, port);
      Client.Credentials = new NetworkCredential(username, password);
      Client.EnableSsl = true;
      From = To = "";
    }


    public void Send(string subject, string body) {
      try { Client.SendAsync(From, To, subject, body, null); }
      catch (Exception) {}
    }
  }


  struct SmtpConfig {
    public string Host;
    public int    Port;
    public string Username;
    public string Password;
    public string From;
    public string To;

    public SmtpConfig(Dictionary<string, string> options, string prefix="") {
      var o = options;
      var p = prefix;
      o.TryGetValue(p+"host", out Host);
      o.TryGetValue(p+"port", out string port);
      o.TryGetValue(p+"username", out Username);
      o.TryGetValue(p+"password", out Password);
      o.TryGetValue(p+"from", out From);
      o.TryGetValue(p+"to", out To);
      int.TryParse(port, out Port);
    }
  }
}

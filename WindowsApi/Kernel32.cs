using System;
using System.Runtime.InteropServices;


namespace KeyLog.WindowsApi {
  class Kernel32 {
    [DllImport("kernel32.dll")]
    public static extern uint GetCurrentThreadId();
    [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr GetModuleHandle(string lpModuleName);
  }
}

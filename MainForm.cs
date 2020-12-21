using System;
using System.Windows.Forms;
using System.Threading;
using KeyLog.Common;


namespace KeyLog {
  public partial class MainForm : Form {

    public MainForm() {
      InitializeComponent();
      Thread t = new Thread(UpdateThread);
      t.IsBackground = true;
      t.Start();
    }

    private void UpdateThread() {
      try {
        while (true) {
          if (!InvokeRequired) UpdateLog();
          else Invoke(new MethodInvoker(UpdateLog));
          Thread.Sleep(100);
        }
      }
      catch (Exception) {}
    }

    private void UpdateLog() {
      TCode.Text = KeyHook.Code.ToString();
      TText.Text = KeyHook.Text.ToString();
    }
  }
}

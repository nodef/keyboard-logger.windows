using System.Runtime.InteropServices;
using System.Drawing;


namespace KeyLog.WindowsApi {
  [StructLayout(LayoutKind.Sequential)]
  public struct POINT {
    public int X;
    public int Y;

    public POINT(int x, int y) {
      this.X = x;
      this.Y = y;
    }
    public POINT(System.Drawing.Point pt) : this(pt.X, pt.Y) {}

    public static implicit operator Point(POINT p) {
      return new System.Drawing.Point(p.X, p.Y);
    }

    public static implicit operator POINT(Point p) {
      return new POINT(p.X, p.Y);
    }
  }
}

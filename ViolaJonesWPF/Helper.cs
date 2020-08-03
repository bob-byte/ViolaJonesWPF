using System;
using System.Drawing;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ViolaJonesWPF
{
    class Helper
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern Boolean DeleteObject(IntPtr handle);

        public static BitmapSource bs;
        public static IntPtr ip;

        public static BitmapSource LoadBitmap(Bitmap source)
        {
            ip = source.GetHbitmap();
            bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(ip, IntPtr.Zero,
                Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            DeleteObject(ip);

            return bs;
        }
    }
}
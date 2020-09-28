using System;
using System.Drawing;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ViolaJonesWPF
{
    public class Helper
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern Boolean DeleteObject(IntPtr handle);

        public static BitmapSource LoadBitmap(Bitmap source)
        {
            IntPtr intPtr = source.GetHbitmap();
            BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(intPtr, IntPtr.Zero,
                Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            DeleteObject(intPtr);

            return bitmapSource;
        }
    }
}
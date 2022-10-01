using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace ViolaJonesWPF
{
    internal static class BitmapExtension
    {
        public static BitmapSource ToBitmapSource(this Bitmap source)
        {
            IntPtr bitmapPointer = source.GetHbitmap();
            BitmapSource bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(
                bitmapPointer,
                palette: IntPtr.Zero,
                sourceRect: Int32Rect.Empty, 
                sizeOptions: BitmapSizeOptions.FromEmptyOptions()
            );
            DeleteObject(bitmapPointer);

            return bitmapSource;
        }

        [DllImport(dllName: "gdi32.dll")]
        private static extern Boolean DeleteObject(IntPtr handle);
    }
}
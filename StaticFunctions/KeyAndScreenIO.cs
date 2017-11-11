using CS3D.dataTypes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// implementation dependent items
/// </summary>
namespace CS3D.StaticFunctions
{
    static class KeyAndScreenIO
    {
        //drawing to screen
        private static Bitmap bitmap = new Bitmap((int) GlobalConstants.SCREEN_WIDTH, (int) GlobalConstants.SCREEN_HEIGHT, GlobalConstants.PIXEL_FORMAT);
        private static Rectangle rectScreen = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="screenArray">What to draw</param>
        /// <param name="drawTo">where to draw it to</param>
        public static unsafe void DrawToScreen(Pixel[,] screenArray, FormMain drawTo)
        {
            //simple error checking to keep program from crashing if array is oversized
            if (screenArray.GetLength(0) != bitmap.Width || screenArray.GetLength(0) != bitmap.Width)
                throw new System.ArgumentException("Parameter array is not the same size as the screen", "original");

            //lock bits to draw screen buffer
            System.Drawing.Imaging.BitmapData bmpData = bitmap.LockBits(rectScreen, System.Drawing.Imaging.ImageLockMode.WriteOnly, bitmap.PixelFormat);

            //Pixel array to bitmap
            UInt32* pixel = (UInt32*)bmpData.Scan0.ToPointer();
            for (uint x = 0; x < bitmap.Width; x++)
                for (uint y = 0; y < bitmap.Height; y++)
                    pixel[y * bitmap.Width + x] = 0xFF000000 | (((UInt32)screenArray[x, y].R) << 16 | (((UInt32)screenArray[x, y].G) << 8)) | (UInt32)screenArray[x, y].B;

            //unlock bits then draw
            bitmap.UnlockBits(bmpData);
            drawTo.DrawImage(bitmap);
        }

        //key IO tools
        public static void UpdateKeyIoArray()
        {
            //system independent implementation of keyIO
        }
    }
}

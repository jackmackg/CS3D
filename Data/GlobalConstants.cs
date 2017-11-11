using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS3D
{
    static class GlobalConstants
    {
        /// <summary>
        /// screen format
        /// </summary>
        public const uint SCREEN_WIDTH = 256;
        public const uint SCREEN_HEIGHT = 224;
        public const PixelFormat PIXEL_FORMAT = PixelFormat.Format32bppArgb;

        /// <summary>
        /// camara settings
        /// </summary>
        public const float FAR_CLIPPING_PLANE = 5000.0f;
        public const float CAMARA_FOV = 90.0f;
    }
}

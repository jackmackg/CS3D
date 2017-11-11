using CS3D.Rendering;
using CS3D.struts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS3D.dataTypes
{
    class Pixel
    {
        private byte r, g, b, a;
        private float depth;
        private float maxDistance;

        public Pixel(byte r, byte g, byte b, byte a, float maxDistance)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;

            this.maxDistance = maxDistance;
            this.depth = this.maxDistance;
        }

        public Pixel(float maxDistance)
        {
            this.r = 0;
            this.g = 0;
            this.b = 0;
            this.a = byte.MaxValue;

            this.maxDistance = maxDistance;
            this.depth = this.maxDistance;
        }

        /// <summary>
        /// force a pixel to be like another one
        /// </summary>
        /// <param name="pixelToCopy"></param>
        public void PixelForceCopy(Pixel pixelToCopy)
        {
            this.r = pixelToCopy.R;
            this.g = pixelToCopy.G;
            this.b = pixelToCopy.B;
            this.a = pixelToCopy.A;
            this.depth = pixelToCopy.Depth;
        }

        /// <summary>
        /// copies a pixel if passes a depth test and aplha test
        /// </summary>
        /// <param name="pixelToCopy"></param>
        public void PixelSetDepthTest(Pixel pixelToCopy)
        {
            if (pixelToCopy.Depth < this.depth && pixelToCopy.A != 0)
                PixelForceCopy(pixelToCopy);
        }

        public void PixelSetDepthTest(Pixel pixelToCopy, float depth)
        {
            if (depth < this.depth && pixelToCopy.A != 0)
            {
                pixelToCopy.depth = depth;
                PixelForceCopy(pixelToCopy);
            }
        }

        /// <summary>
        /// set pixels back to defalt values. used to clear the screen after drawing
        /// </summary>
        public void ClearPixel()
        {
            this.r = 0;
            this.g = 0;
            this.b = 0;
            this.a = byte.MaxValue;
            this.depth = this.maxDistance;
        }

        public byte R { get { return r; } }
        public byte G { get { return g; } }
        public byte B { get { return b; } }
        public byte A { get { return a; } }
        public float Depth { get { return depth; } }
    }
}

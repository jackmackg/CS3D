using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS3D.struts;
using CS3D.dataTypes;

namespace CS3D.Rendering
{
    /// <summary>
    /// Holds data about the camara and the screen array to render to
    /// </summary>
    class Camara
    {
        public Vector3 location;
        public Vector3 angle;
        public Pixel[,] screen;

        private float fov; //holds calculated fov
        private float farClippingPlane;

        public Camara(Vector3 location, Vector3 angle, uint height, uint width, float farClippingPlane, float fov)
        {
            this.location = location;
            this.angle = angle;
            this.SetFov(fov);
            this.farClippingPlane = farClippingPlane;

            screen = new Pixel[width,height];
            for (uint x = 0; x < width; x++)
                for (uint y = 0; y < height; y++)
                    screen[x, y] = new Pixel(farClippingPlane);
        }

        public float FarClippingPlane { get { return farClippingPlane; } }

        //fov tools
        /// <summary>
        /// set the fov of the camara, does the background calculation needed
        /// </summary>
        /// <param name="angle">fov in degrees, fov is set off screen width</param>
        public void SetFov(float angle)
        {
            this.fov = (float)((double)GetScreenWidth() / Math.Tan(StaticTools.DegreesToRadians(angle / 2.0f)));
        }

        /// <summary>
        /// Used when drawing a transformed triangle. This value is not the FOV set
        /// </summary>
        /// <returns>fov precalculated for render engine</returns>
        public float GetCalculatedFov()
        {
            return fov;
        }

        //screen tools
        public uint GetScreenWidth()
        {
            return (uint)screen.GetLength(0);
        }

        public uint GetScreenHeight()
        {
            return (uint)screen.GetLength(1);
        }

        public void ClearScreen()
        { //possible multithreading
            foreach (Pixel i in screen)
                i.ClearPixel();
        }
    }
}

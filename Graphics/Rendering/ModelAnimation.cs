using CS3D.dataTypes;
using CS3D.DataTypes.FundamentalGraphics;
using CS3D.StaticData;
using CS3D.struts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS3D.Rendering
{
    /// <summary>
    /// Holds a single Animation. Can hold a single model if needed, just load only one frame.
    /// Only one texture perAnimation but UVs may change eatch frame. You may use large texture and animate UVs
    /// </summary>
    class ModelAnimation
    {
        public Vector3 location;
        public Vector3 angle;
        public float scale;
        public bool orthographic; //use this for 2D graphics
        public readonly Pixel[,] texture;
        private List<Triangle[]> fames;

        //timer values
        private uint currentFrame;
        private Int64 frameTimer = 0;
        public Int64 frameTimeMs;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="location">in local space</param>
        /// <param name="angle">int local space</param>
        /// <param name="orthographic">used for 2D graphics</param>
        /// <param name="scale"></param>
        /// <param name="texture"></param>
        /// <param name="frameTimeMs">how long to wait till it moves onto the next frame</param>
        public ModelAnimation(Vector3 location, Vector3 angle, bool orthographic, float scale, Pixel[,] texture, Int64 frameTimeMs)
        {
            this.location = location;
            this.angle = angle;
            this.orthographic = orthographic;
            this.scale = scale;
            this.texture = texture;
            this.frameTimeMs = frameTimeMs;
            this.fames = new List<Triangle[]>();
        }

        public void Render(Camara cam)
        {
            foreach (Triangle i in fames[(int)currentFrame])
                i.Render(cam, this);
        }

        public uint GetTextureWidth()
        {
            return (uint)texture.GetLength(0);
        }

        public uint GetTextureHeight()
        {
            return (uint)texture.GetLength(1);
        }

        /// <summary>
        /// call this eatch frame to check if the animation should move to the next frame, it will update if its time
        /// </summary>
        /// <returns>true if last frame is done showing</returns>
        public bool AnimationUpdate()
        {
            if (StaticTools.TimerPassed(frameTimer))
            {
                frameTimer = StaticTools.TimerSet(frameTimeMs);

                if (++currentFrame >= this.GetAnimationLength())
                {
                    currentFrame--;
                    return true;
                }
                else
                    return false;
            }

            return false;
        }

        /// <summary>
        /// Starts the Animation from the first frame
        /// </summary>
        public void AnimationStart()
        {
            frameTimer = StaticTools.TimerSet(frameTimeMs);
            currentFrame = 0;
        }

        /// <summary>
        /// How many frames of animation
        /// </summary>
        public uint GetAnimationLength()
        {
            return (uint)this.fames.Count;
        }
        

        public void AddFrameFromObj(string fileName)
        {
            this.fames.Add(ObjParser.LoadObj(fileName));
        }
    }
}

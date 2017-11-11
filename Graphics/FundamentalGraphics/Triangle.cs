using CS3D.Rendering;
using CS3D.struts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS3D.DataTypes.FundamentalGraphics
{
    class Triangle
    {
        //transformed values
        private Vector3 Vertex1;
        private Vector2 Uv1;

        private Vector3 Vertex2;
        private Vector2 Uv2;

        private Vector3 Vertex3;
        private Vector2 Uv3;

        public Triangle(Vector3 Vertex1, Vector2 Uv1, Vector3 Vertex2, Vector2 Uv2, Vector3 Vertex3, Vector2 Uv3)
        {
            this.Vertex1 = Vertex1;
            this.Uv1 = Uv1;

            this.Vertex2 = Vertex2;
            this.Uv2 = Uv2;

            this.Vertex3 = Vertex3;
            this.Uv3 = Uv3;
        }

        //render methods
        public void Render(Camara cam, ModelAnimation model)
        {
            //make a copy, don't touch original points to keep accuracy
            Vector3 tmpVertex1 = Vertex1;
            Vector3 tmpVertex2 = Vertex2;
            Vector3 tmpVertex3 = Vertex3;

            //move the vertexs in local space
            ScaleVertexs(ref tmpVertex1, ref tmpVertex2, ref tmpVertex3, model.scale);
            TranslateVertexs(ref tmpVertex1, ref tmpVertex2, ref tmpVertex3, model.location);
            RotateVertexs(ref tmpVertex1, ref tmpVertex2, ref tmpVertex3, model.angle);

            //move the vertexs in world space
            TranslateVertexs(ref tmpVertex1, ref tmpVertex2, ref tmpVertex3, cam.location);
            RotateVertexs(ref tmpVertex1, ref tmpVertex2, ref tmpVertex3, cam.angle);

            //culling/drawing/perspective
            if (!InZRange(tmpVertex1, tmpVertex2, tmpVertex1, cam))
                return;

            if (!model.orthographic)
                PerspectiveVertexs(ref tmpVertex1, ref tmpVertex2, ref tmpVertex3, cam);

            if (!OnScreen(tmpVertex1, tmpVertex2, tmpVertex1, cam))
                return;

            DrawTriangle(tmpVertex1, tmpVertex2, tmpVertex1, model, cam);
        }

        //triangle tools
        private void RotateVertexs(ref Vector3 tmpVertex1, ref Vector3 tmpVertex2, ref Vector3 tmpVertex3, Vector3 angle)
        {
            float angle1Sin = (float)Math.Sin(angle.x);
            float angle1Cos = (float)Math.Cos(angle.x);

            float angle2Sin = (float)Math.Sin(angle.y);
            float angle2Cos = (float)Math.Cos(angle.y);

            float angle3Sin = (float)Math.Sin(angle.z);
            float angle3Cos = (float)Math.Cos(angle.z);

            float x1 = tmpVertex1.x;
            float y1 = tmpVertex1.y;
            float z1 = tmpVertex1.z;

            float x2 = tmpVertex2.x;
            float y2 = tmpVertex2.y;
            float z2 = tmpVertex2.z;

            float x3 = tmpVertex3.x;
            float y3 = tmpVertex3.y;
            float z3 = tmpVertex3.z;

            float returnX1;
            float returnY1;
            float returnZ1;

            float returnX2;
            float returnY2;
            float returnZ2;

            float returnX3;
            float returnY3;
            float returnZ3;


            //point 1
            returnX1 = x1 * angle1Cos - y1 * angle1Sin;
            y1 = x1 * angle1Sin + y1 * angle1Cos;

            returnY1 = y1 * angle2Cos - z1 * angle2Sin;
            z1 = y1 * angle2Sin + z1 * angle2Cos;

            returnZ1 = z1 * angle3Cos - returnX1 * angle3Sin;
            returnX1 = z1 * angle3Sin + returnX1 * angle3Cos;

            //point 2
            returnX2 = x2 * angle1Cos - y2 * angle1Sin;
            y2 = x2 * angle1Sin + y2 * angle1Cos;

            returnY2 = y2 * angle2Cos - z2 * angle2Sin;
            z2 = y2 * angle2Sin + z2 * angle2Cos;

            returnZ2 = z2 * angle3Cos - returnX2 * angle3Sin;
            returnX2 = z2 * angle3Sin + returnX2 * angle3Cos;

            //point 3
            returnX3 = x3 * angle1Cos - y3 * angle1Sin;
            y3 = x3 * angle1Sin + y3 * angle1Cos;

            returnY3 = y3 * angle2Cos - z3 * angle2Sin;
            z3 = y3 * angle2Sin + z3 * angle2Cos;

            returnZ3 = z3 * angle3Cos - returnX3 * angle3Sin;
            returnX3 = z3 * angle3Sin + returnX3 * angle3Cos;


            //copy back
            tmpVertex1.x = returnX1;
            tmpVertex1.y = returnY1;
            tmpVertex1.z = returnZ1;

            tmpVertex2.x = returnX2;
            tmpVertex2.y = returnY2;
            tmpVertex2.z = returnZ2;

            tmpVertex3.x = returnX3;
            tmpVertex3.y = returnY3;
            tmpVertex3.z = returnZ3;
        }

        private void TranslateVertexs(ref Vector3 tmpVertex1, ref Vector3 tmpVertex2, ref Vector3 tmpVertex3, Vector3 delta)
        {
            tmpVertex1.x += delta.x;
            tmpVertex1.y += delta.y;
            tmpVertex1.z += delta.z;

            tmpVertex2.x += delta.x;
            tmpVertex2.y += delta.y;
            tmpVertex2.z += delta.z;

            tmpVertex3.x += delta.x;
            tmpVertex3.y += delta.y;
            tmpVertex3.z += delta.z;
        }

        private void ScaleVertexs(ref Vector3 tmpVertex1, ref Vector3 tmpVertex2, ref Vector3 tmpVertex3, float scale)
        {
            tmpVertex1.x *= scale;
            tmpVertex1.y *= scale;
            tmpVertex1.z *= scale;

            tmpVertex2.x *= scale;
            tmpVertex2.y *= scale;
            tmpVertex2.z *= scale;

            tmpVertex3.x *= scale;
            tmpVertex3.y *= scale;
            tmpVertex3.z *= scale;
        }

        private void PerspectiveVertexs(ref Vector3 tmpVertex1, ref Vector3 tmpVertex2, ref Vector3 tmpVertex3, Camara cam)
        {
            float percentCloseToCamera;
            uint halfScreenWidth = cam.GetScreenWidth() / 2;
            uint halfScreenHeight = cam.GetScreenWidth() / 2;

            if (tmpVertex1.z > 0)
            {
                percentCloseToCamera = tmpVertex1.z / (tmpVertex1.z + cam.GetCalculatedFov());

                tmpVertex1.x += (halfScreenWidth - tmpVertex1.x) * percentCloseToCamera;
                tmpVertex1.y += (halfScreenHeight - tmpVertex1.y) * percentCloseToCamera;
            }

            if (tmpVertex2.z > 0)
            {
                percentCloseToCamera = tmpVertex2.z / (tmpVertex2.z + cam.GetCalculatedFov());

                tmpVertex2.x += (halfScreenWidth - tmpVertex2.x) * percentCloseToCamera;
                tmpVertex2.y += (halfScreenHeight - tmpVertex2.y) * percentCloseToCamera;
            }

            if (tmpVertex3.z > 0)
            {
                percentCloseToCamera = tmpVertex3.z / (tmpVertex3.z + cam.GetCalculatedFov());

                tmpVertex3.x += (halfScreenWidth - tmpVertex3.x) * percentCloseToCamera;
                tmpVertex3.y += (halfScreenHeight - tmpVertex3.y) * percentCloseToCamera;
            }
        }

        private bool InZRange(Vector3 tmpVertex1, Vector3 tmpVertex2, Vector3 tmpVertex3, Camara cam)
        {
            //check if in camara z range
            if (MaxZ(tmpVertex1, tmpVertex2, tmpVertex3) < 0 || MinZ(tmpVertex1, tmpVertex2, tmpVertex3) > cam.FarClippingPlane)
                return false;

            //check if facing away from the camara
            if (Area(tmpVertex1, tmpVertex2, tmpVertex3) < -1)
                return false;

            return true;
        }

        private bool OnScreen(Vector3 tmpVertex1, Vector3 tmpVertex2, Vector3 tmpVertex3, Camara cam)
        {
            //check if its in the window box area
            if (Left(tmpVertex1, tmpVertex2, tmpVertex3) > cam.GetScreenWidth())
                return false;

            if (Right(tmpVertex1, tmpVertex2, tmpVertex3) < 0)
                return false;

            if (Top(tmpVertex1, tmpVertex2, tmpVertex3) > cam.GetScreenHeight())
                return false;

            if (Bottom(tmpVertex1, tmpVertex2, tmpVertex3) < 0)
                return false;

            return true;
        }

        private float MaxZ(Vector3 tmpVertex1, Vector3 tmpVertex2, Vector3 tmpVertex3)
        {
            return Math.Max(tmpVertex1.z, Math.Max(tmpVertex2.z, tmpVertex3.z));
        }

        private float MinZ(Vector3 tmpVertex1, Vector3 tmpVertex2, Vector3 tmpVertex3)
        {
            return Math.Min(tmpVertex1.z, Math.Min(tmpVertex2.z, tmpVertex3.z));
        }

        private int Area(Vector3 tmpVertex1, Vector3 tmpVertex2, Vector3 tmpVertex3)
        {
            int x1 = (int)tmpVertex1.x;
            int y1 = (int)tmpVertex1.y;

            int x2 = (int)tmpVertex2.x;
            int y2 = (int)tmpVertex2.y;

            int x3 = (int)tmpVertex3.x;
            int y3 = (int)tmpVertex3.y;

            int y2my3 = y1 - y3;
            int y3my1 = y3 - y1;
            int y1my2 = y1 - y2;

            return x1 * (y2my3) + x2 * (y3my1) + x3 * (y1my2);
        }

        private float Left(Vector3 tmpVertex1, Vector3 tmpVertex2, Vector3 tmpVertex3)
        {
            return Math.Min(tmpVertex1.x, Math.Min(tmpVertex2.x, tmpVertex3.x));
        }

        private float Right(Vector3 tmpVertex1, Vector3 tmpVertex2, Vector3 tmpVertex3)
        {
            return Math.Max(tmpVertex1.x, Math.Max(tmpVertex2.x, tmpVertex3.x));
        }

        private float Top(Vector3 tmpVertex1, Vector3 tmpVertex2, Vector3 tmpVertex3)
        {
            return Math.Min(tmpVertex1.y, Math.Min(tmpVertex2.y, tmpVertex3.y));
        }

        private float Bottom(Vector3 tmpVertex1, Vector3 tmpVertex2, Vector3 tmpVertex3)
        {
            return Math.Max(tmpVertex1.y, Math.Max(tmpVertex2.y, tmpVertex3.y));
        }

        private void DrawTriangle(Vector3 tmpVertex1, Vector3 tmpVertex2, Vector3 tmpVertex3, ModelAnimation mod, Camara cam)
        {
            //set array boundes
            int left = (int)Math.Max(0, Left(tmpVertex1, tmpVertex2, tmpVertex3));
            int right = (int)Math.Min(cam.GetScreenWidth(), Right(tmpVertex1, tmpVertex2, tmpVertex3));
            int top = (int)Math.Max(0, Top(tmpVertex1, tmpVertex2, tmpVertex3));
            int bottom = (int)Math.Min(cam.GetScreenWidth(), Bottom(tmpVertex1, tmpVertex2, tmpVertex3));

            //setup values for the draw loop
            int x1 = (int)tmpVertex1.x;
            int y1 = (int)tmpVertex1.y;
            float z1 = tmpVertex1.z;

            int x2 = (int)tmpVertex2.x;
            int y2 = (int)tmpVertex2.y;
            float z2 = tmpVertex2.z;

            int x3 = (int)tmpVertex3.x;
            int y3 = (int)tmpVertex3.y;
            float z3 = tmpVertex3.z;

            //percomputer values
            float totalArea = -Area(tmpVertex1, tmpVertex2, tmpVertex3);

            int y2my3 = y2 - y3;
            int y3my1 = y3 - y1;
            int y1my2 = y1 - y2;

            //computing uvs
            float uvX1 = Uv1.u * mod.GetTextureWidth();
            float uvX2 = Uv2.u * mod.GetTextureWidth();
            float uvX3 = Uv3.u * mod.GetTextureWidth();

            float uvY1 = Uv1.v * mod.GetTextureHeight();
            float uvY2 = Uv2.v * mod.GetTextureHeight();
            float uvY3 = Uv3.v * mod.GetTextureHeight();

            //draw loop
            for (int y = top; y < bottom; y++)
            {
                int apre = x2 * (y3 - y) + x3 * (y - y2);
                int bpre = x1 * (y - y3) + x3 * (y1 - y);
                int cpre = x1 * (y2 - y) + x2 * (y - y1);

                for (int x = left; x < right; x++)
                {
                    float ba = Math.Abs(x * y2my3 + apre) / totalArea;
                    float bb = Math.Abs(x * y3my1 + bpre) / totalArea;
                    float bc = Math.Abs(x * y1my2 + cpre) / totalArea;

                    //test if point is in triangle
                    if (ba + bb + bc <= totalArea)
                    {
                        //get the uv/depth
                        int u = (int)(uvY1 * ba + uvY2 * bb + uvY3 * bc);
                        int v = (int)(uvX1 * ba + uvX2 * bb + uvX3 * bc);
                        float depth = z1 * ba + z2 * bb + z3 * bc;

                        //set the pixel if passes depth test
                        cam.screen[x, y].PixelSetDepthTest(mod.texture[u, v], depth);
                    }
                }
            }
        }
    }
}

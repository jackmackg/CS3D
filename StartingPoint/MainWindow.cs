using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Resources;
using CS3D.dataTypes;

namespace CS3D
{
    public partial class FormMain : Form
    {
        private Graphics screenMain;

        public FormMain()
        {
            InitializeComponent();

            //make buffer that will be drawn on screen
            screenMain = pictureBoxMain.CreateGraphics();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
        }

        public void DrawImage(Bitmap img)
        {
            screenMain.DrawImage(img, 0, 0);
        }

        //test draw function
        void FormMain_KeyPress(object sender, KeyPressEventArgs e)
        {
        }
    }
}

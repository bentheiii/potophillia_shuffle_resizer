using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhotophilliaShuffleResizer
{
    public partial class ImagView : Form
    {
        private Point cursoradjust = new Point(-1, -1), formadjust;
        public ImagView(Image i)
        {
            InitializeComponent();
            this.Size = i.Size;
            pictureBox1.Image = i;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Close();
        }
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            cursoradjust = Cursor.Position;
            formadjust = this.Location;
        }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (cursoradjust.X != -1)
            {
                this.Location = new Point(formadjust.X + Cursor.Position.X - cursoradjust.X, formadjust.Y + Cursor.Position.Y - cursoradjust.Y);
            }
        }

        private void ImagView_KeyUp(object sender, KeyEventArgs e)
        {
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
        }
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            cursoradjust = new Point(-1, -1);
        }
    }
}

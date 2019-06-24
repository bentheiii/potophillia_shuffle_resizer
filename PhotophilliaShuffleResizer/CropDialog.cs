using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FormStone;
using PhotophilliaShuffleResizer.Properties;

namespace PhotophilliaShuffleResizer
{
    public partial class CropDialog : ReturnForm<Rectangle>
    {
        private readonly Image _source;
        public CropDialog(Image source)
        {
            _source = source;
            InitializeComponent();
        }
        private int _top = 0, _bottom = 0, _left = 0, _right = 0;
        public void Render()
        {
            Bitmap ret = new Bitmap(_source);
            using (Graphics g = Graphics.FromImage(ret))
            {

                /* convention: horizontals take care of the diagonal greys:
                 * ++++++
                 * ++++++
                 * ++  ++
                 * ++  ++
                 * ++  ++
                 * ++++++
                 * ++++++
                 * 
                 * is handled via:
                 * LLTTRR
                 * LLTTRR
                 * LL  RR
                 * LL  RR
                 * LL  RR
                 * LLBBRR
                 * LLBBRR
                 */
                var grays = new List<Rectangle>();
                Brush b = new SolidBrush(Color.FromArgb(200, Color.Gray));
                if (_top > 0)
                {
                    grays.Add(new Rectangle
                    {
                        X = _left,
                        Y = 0,
                        Width = _source.Width - _left - _right,
                        Height = _top
                    });
                }
                if (_bottom > 0)
                {
                    grays.Add(new Rectangle
                    {
                        X = _left,
                        Y = _source.Height - _bottom,
                        Width = _source.Width - _left - _right,
                        Height = _bottom
                    });
                }
                if (_left > 0)
                {
                    grays.Add(new Rectangle
                    {
                        X = 0,
                        Y = 0,
                        Width = _left,
                        Height = _source.Height
                    });
                }
                if (_right > 0)
                {
                    grays.Add(new Rectangle
                    {
                        X = _source.Width - _right,
                        Y = 0,
                        Width = _right,
                        Height = _source.Height
                    });
                }
                if (grays.Any())
                {
                    g.FillRectangles(b,grays.ToArray());
                }
            }
            pictureBox1.Image?.Dispose();
            pictureBox1.Image = ret;
        }


        private void CropDialog_Load(object sender, EventArgs e)
        {
            if (!Settings.Default.CropperRectangle.IsEmpty)
            {
                this.Location = Settings.Default.CropperRectangle.Location;
                this.Size = Settings.Default.CropperRectangle.Size;
            }
            Render();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            var ret = new Rectangle(_left, _top, _source.Width - _left - _right, _source.Height - _top - _bottom);
            if (ret.Height*ret.Width == 0)
            {
                MessageBox.Show("Cropped image cannot be empty");
                return;
            }
            
            Close(ret);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Close(DialogResult.Cancel);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                _left = _right = Math.Max(_left, _right);
                RightBar.Value = (int)((1 - 2 * (_left/(double)_source.Width))*RightBar.Maximum);
                LeftBar.Value = (int)((2 * (_left / (double)_source.Width)) * LeftBar.Maximum);
                Render();
            }
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                _top = _bottom = Math.Max(_top, _bottom);
                TopBar.Value = (int)((1 - 2 * (_top / (double)_source.Height)) * TopBar.Maximum);
                BottomBar.Value = (int)(2 * (_top / (double)_source.Height) * BottomBar.Maximum);
                Render();
            }
        }

        private void RightBar_Scroll(object sender, EventArgs e)
        {
            _right = (int)(_source.Width / 2 * (1.0-RightBar.Value / (double)RightBar.Maximum));
            if (checkBox1.Checked)
            {
                _left = _right;
                LeftBar.Value = (int)(( 2 * (_left / (double)_source.Width)) * LeftBar.Maximum);
            }
            Render();
        }

        private void CropDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            Point loc = this.Location;
            var s = this.WindowState == FormWindowState.Normal ? this.Size : this.RestoreBounds.Size;
            Settings.Default.CropperRectangle = new Rectangle(loc, s);
            Settings.Default.Save();

            pictureBox1.Image?.Dispose();
        }

        private void LeftBar_Scroll(object sender, EventArgs e)
        {
            _left = (int)(_source.Width / 2 * (LeftBar.Value / (double)LeftBar.Maximum));
            if (checkBox1.Checked)
            {
                _right = _left;
                RightBar.Value = (int)((1 - 2 * (_left / (double)_source.Width)) * RightBar.Maximum);
            }
            Render();
        }
        private void BottomBar_Scroll(object sender, EventArgs e)
        {
            _bottom = (int)(_source.Height/2*(BottomBar.Value/ (double)BottomBar.Maximum));
            if (checkBox2.Checked)
            {
                _top = _bottom;
                TopBar.Value = (int)((1 - 2 * (_top / (double)_source.Height)) * TopBar.Maximum);
            }
            Render();
        }
        private void TopBar_Scroll(object sender, EventArgs e)
        {
            _top = (int)(_source.Height/2*(1.0 - (TopBar.Value/ (double)TopBar.Maximum)));
            if (checkBox2.Checked)
            {
                _bottom = _top;
                BottomBar.Value = (int)(2 * (_top / (double)_source.Height) * BottomBar.Maximum);
            }
            Render();
        }
    }
}

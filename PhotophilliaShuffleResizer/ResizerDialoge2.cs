using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FormStone;
using PhotophilliaShuffleResizer.Properties;

namespace PhotophilliaShuffleResizer
{
    public partial class ResizerDialoge2 : ReturnForm<Rectangle>
    {
        private readonly string _dstpath;
        private readonly ResizingProfile _profile;
        private Color _backgroundColor;
        private Bitmap _croppedImage;
        private readonly Bitmap _sourceimage;
        private Rectangle _cropRectangle;
        public ResizerDialoge2(Bitmap scrimage, string dstpath, ResizingProfile profile) : this(scrimage, dstpath, profile, Rectangle.Empty)
        { }
        public ResizerDialoge2(Bitmap srcimage, string dstpath, ResizingProfile profile, Rectangle crop)
        {
            _sourceimage = srcimage;
            _dstpath = dstpath;
            _profile = profile;
            _cropRectangle = crop;
            InitializeComponent();
        }
        public void SetColor(Color c, bool render = true)
        {
            _backgroundColor = c;
            colorWheelCtrl1.SelectedColor = c;
            eyedropColorPicker1.SelectedColor = c;
            if (render)
                Render();
        }
        private double _scaleMin, _scaleMax, _zoom, _crop, _native;

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            Magnitude = (trackBar1.Value / (double)trackBar1.Maximum) * (_scaleMax - _scaleMin) + _scaleMin;
            Render();
        }

        private void ZoomLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Magnitude = _zoom;
            Render();
        }

        private void CropLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Magnitude = _crop;
            Render();
        }

        private void NativeLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Magnitude = _native;
            Render();
        }

        private double _mag;
        private double Magnitude
        {
            get
            {
                return _mag;
            }
            set
            {
                _mag = value;
                trackBar1.Value = (int)((value - _scaleMin)/(_scaleMax - _scaleMin)*trackBar1.Maximum);
                pictureBox2.Visible = pictureBox2.Enabled = (_mag > _zoom);
            }
        }
        public Bitmap outImage(bool gap = false)
        {
            Rectangle intersect;
            Rectangle x, y;
            return outImage(out intersect, out x, out y, gap);
        }
        public Bitmap outImage(out Rectangle intersect, out Rectangle dstRectangle, out Rectangle srcRectangle, bool gap = false)
        {
            Bitmap ret = new Bitmap(_profile.width,_profile.height + (gap ? _profile.bottomgap + _profile.topgap : 0));
            using (Graphics g = Graphics.FromImage(ret))
            {
                //background
                using (Brush b = new SolidBrush(_backgroundColor))
                {
                    g.FillRectangle(b,0,0,_profile.width,_profile.height);
                }

                srcRectangle = getSrcRect();
                int xgap, ygap;
                dstRectangle = getDstRect(out xgap, out ygap);

                var target = new Rectangle((int)(-xgap),(int)(-ygap),_profile.width,_profile.height);

                intersect = Rectangle.Intersect(new Rectangle(new Point(0,0), dstRectangle.Size), target);

                if (gap && _profile.topgap > 0)
                    dstRectangle.Y += _profile.topgap;

                g.DrawImage(_croppedImage, dstRectangle, srcRectangle, GraphicsUnit.Pixel);

                //gap
                if (gap)
                {
                    using (Brush b = new SolidBrush(Color.Black))
                    {
                        if (_profile.bottomgap > 0)
                            g.FillRectangle(b, 0, _profile.height, _profile.width, _profile.bottomgap);
                        if (_profile.topgap > 0)
                            g.FillRectangle(b, 0, 0, _profile.width, _profile.topgap);
                    }
                }
                g.Save();
            }
            return ret;
        }
        public Bitmap OutSubImage(Rectangle intersect)
        {
            double cropToSubviewRatio = Math.Min(pictureBox2.Width/(double)_croppedImage.Width, pictureBox2.Height/(double)_croppedImage.Height);
            var ret = new Bitmap((int)(_croppedImage.Width * cropToSubviewRatio * Magnitude), (int)(_croppedImage.Height * cropToSubviewRatio * Magnitude));
            using (Graphics g = Graphics.FromImage(ret))
            {
                g.DrawImage(_croppedImage,new Rectangle(0,0,ret.Width,ret.Height));
                using (Pen p = new Pen(Color.Red))
                {
                    Rectangle subSrc = new Rectangle((int)(intersect.X*cropToSubviewRatio), (int)(intersect.Y*cropToSubviewRatio), (int)(intersect.Width * cropToSubviewRatio), (int)(intersect.Height * cropToSubviewRatio));
                    g.DrawRectangle(p,subSrc);
                }
                g.Save();
            }
            return ret;
        }
        private Rectangle getSrcRect()
        {
            Rectangle ground = new Rectangle(0,0,_croppedImage.Width,_croppedImage.Height);
            return ground;
        }
        private Rectangle getDstRect(out int xgap, out int ygap)
        {
            xgap = (int)((_profile.width - (int)(_croppedImage.Width*Magnitude))*(trackBar2.Value/100.0));
            ygap = (int)((_profile.height - (int)(_croppedImage.Height*Magnitude))*((trackBar3.Maximum-trackBar3.Value)/100.0));
            Rectangle filter = new Rectangle(xgap, ygap, (int)(_croppedImage.Width * Magnitude), (int)(_croppedImage.Height * Magnitude));
            return filter;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            trackBar2.Value = trackBar2.Maximum / 2;
            trackBar3.Value = trackBar3.Maximum / 2;
            Render();
        }

        private void colorWheelCtrl1_Load(object sender, EventArgs e)
        {
            SetColor(colorWheelCtrl1.SelectedColor);
        }

        private void eyedropColorPicker1_Click(object sender, EventArgs e)
        {
            SetColor(eyedropColorPicker1.SelectedColor);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            var dog = new CropDialog(_sourceimage);
            var response = dog.ShowDialog();
            if (response.Item1 == DialogResult.OK)
            {
                _cropRectangle = response.Item2;
                _croppedImage = _sourceimage.Clone(_cropRectangle, _sourceimage.PixelFormat);
                BuildScale();
                Render();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var im = outImage(true))
            {
                im.Save(_dstpath);
                Close(DialogResult.OK, _cropRectangle);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close(DialogResult.Ignore);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close(DialogResult.Abort);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Close(DialogResult.Retry);
        }

        private void ResizerDialoge2_FormClosing(object sender, FormClosingEventArgs e)
        {
            Point loc = this.Location;
            var s = this.WindowState == FormWindowState.Normal ? this.Size : this.RestoreBounds.Size;
            Settings.Default.ResizerRectangle = new Rectangle(loc,s);
            Settings.Default.Save();
            
            pictureBox1.Image?.Dispose();
            pictureBox2.Image?.Dispose();
            _croppedImage.Dispose();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            using (var bitmap = outImage())
            {
                ImagView view;
                view = new ImagView(bitmap);
                view.ShowDialog();
            }
        }

        private void ResizerDialoge2_Load(object sender, EventArgs e)
        {
            if (!Settings.Default.ResizerRectangle.IsEmpty)
            {
                this.Location = Settings.Default.ResizerRectangle.Location;
                this.Size = Settings.Default.ResizerRectangle.Size;
            }
            this.Text = _dstpath;
            _croppedImage = _cropRectangle.IsEmpty ? _sourceimage.Clone(new Rectangle(0,0,_sourceimage.Width,_sourceimage.Height), _sourceimage.PixelFormat) : _sourceimage.Clone(_cropRectangle, _sourceimage.PixelFormat);
            BuildScale();
            SetColor(Color.Black, false);
            Render();
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            Render();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            ColorDialog d = new ColorDialog();
            if (d.ShowDialog() == DialogResult.OK)
                SetColor(d.Color);
        }

        public void Render()
        {
            pictureBox1.Image?.Dispose();
            Rectangle intersect;
            Rectangle src, dst;
            pictureBox1.Image = outImage(out intersect, out dst, out src);

            int xgap = (int)(_profile.width - src.Width * Magnitude);
            int ygap = (int)(_profile.height - src.Height * Magnitude);

            int wastepix=0, hidpix=0;

            if (xgap > 0)
                wastepix += (xgap)*_profile.height;
            else
                hidpix += (int)((-xgap)*_croppedImage.Height*Magnitude);

            if (ygap > 0)
                wastepix += (ygap) * _profile.width;
            else
                hidpix += (int)((-ygap) * _croppedImage.Width*Magnitude);

            double waste = wastepix/(double)(_profile.width*_profile.height);
            double hid = hidpix/(double)(_croppedImage.Width*_croppedImage.Height * Magnitude * Magnitude);

            diagLabel.Text = $"Wasted space: {waste:p}\n Image Hidden: {hid:p}";

            pictureBox2.Image?.Dispose();
            pictureBox2.Image = Magnitude > _zoom ? OutSubImage(intersect) : null;
        }
        public void BuildScale()
        {
            int cw = _croppedImage.Width;
            int ch = _croppedImage.Height;
            int tw = _profile.width;
            int th = _profile.height;

            _native = 1;
            double matchHeight = th/(double)ch;
            double matchWidth = tw/(double)cw;
            if (matchHeight > matchWidth)
            {
                _zoom = matchWidth;
                _crop = matchHeight;
            }
            else
            {
                _crop = matchWidth;
                _zoom = matchHeight;
            }
            _scaleMin = new[] {_native, _zoom, _crop}.Min();
            _scaleMax = new[] {_native, _zoom, _crop}.Max();

            if (Math.Abs(_scaleMax - _scaleMin) < 0.001)
            {
                _scaleMax += 0.2;
                _scaleMin -= 0.2;
            }

            placeLink(NativeLink,"Native (100%)", _native);
            placeLink(CropLink,$"Crop ({_crop:p})", _crop);
            placeLink(ZoomLink, $"Zoom ({_zoom:p})", _zoom);
            if (_zoom > _profile.maxZoom)
            {
                Magnitude = _native;
            }
            else
            {
                if (_crop/_zoom < _profile.maxCropCoefficient && _crop < _profile.maxZoom)
                {
                    Magnitude = _crop;
                }
                else
                {
                    Magnitude = _zoom;
                }
            }
        }
        public void placeLink(LinkLabel link, string label, double value)
        {
            double fraction = 1 - (value - _scaleMin)/(_scaleMax - _scaleMin);
            int pixels = (int)(fraction*trackBar1.Height);
            link.Location = new Point(link.Location.X, trackBar1.Location.Y+pixels);

            link.Text = label;
        }
    }
}

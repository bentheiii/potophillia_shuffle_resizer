using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using WhetStone.Random;

namespace PhotophilliaShuffleResizer
{
    public partial class JobMenuControl : UserControl
    {
        private ResizingJob _job;
        //todo everything djobs should be in a seperate class
        private readonly object _joblock = new object();
        private readonly object _djoblock = new object();
        private readonly ISet<DownloadJob> _djobs = new HashSet<DownloadJob>();
        public JobMenuControl()
        {
            InitializeComponent();
        }
        public event EventHandler BackPressed;
        public ResizingJob job
        {
            private get
            {
                return _job;
            }
            set
            {
                lock (_joblock)
                {
                    _job = value;
                }
                refreshInfo();
            }
        }
        private void refreshInfo()
        {
            lock (_joblock)
            {
                label1.Text = job.info();
            }
        }
        private void backButton_Click(object sender, EventArgs e)
        {
            BackPressed.Invoke(this,e);
        }

        private void poolButton_Click(object sender, EventArgs e)
        {
            lock (_joblock)
            {
                job.rePoolDests();
                job.rePoolImages(new[] {"*.bmp", "*.jpg", "*.png"});
                job.save();
            }
            refreshInfo();
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            IList<Tuple<string, string>> list;
            lock (_joblock)
            {
                list = job.getPairs();
            }
            var gen = new LocalRandomGenerator();
            while (list.Any())
            {
                int randindex = gen.Int(list.Count);

                var dest = list[randindex].Item2;
                var file = list[randindex].Item1;

                var dstpath = Path.Combine(dest, file);
                string srcpath;
                lock (_joblock)
                {
                    srcpath = Path.Combine(job.rawdir, file);
                }

                using (var image = Image.FromFile(srcpath))
                {
                    using (var bmp = new Bitmap(image))
                    {
                        ResizingProfile prof;
                        lock (_joblock)
                        {
                            prof = job.getProfile(dest);
                        }
                        var diag = new ResizerDialoge2(bmp, dstpath, prof);
                        var result = DialogResult.Retry;
                        while (result == DialogResult.Retry)
                        {
                            result = diag.ShowDialog().Item1;
                        }
                        if (result == DialogResult.Cancel)
                            break;
                        lock (_joblock)
                        {
                            job.set(dest, file, result);
                        }
                        if (result == DialogResult.Abort)
                            break;
                        //todo make this a new function
                        {
                            list[randindex] = list.Last();
                            list.RemoveAt(list.Count-1);
                        }
                        refreshInfo();
                    }
                }
            }
            refreshInfo();
            lock (_joblock)
            {
                job.save();
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() != DialogResult.OK)
                return;
            lock (_joblock)
            {   
                job.sourcePath = saveFileDialog1.FileName;
                job.save();
            }
        }

        private void RAWButton_Click(object sender, EventArgs e)
        {
            lock (_joblock)
            {
                Process.Start(job.rawdir);
            }
        }

        private void JobMenuControl_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.Text) ? DragDropEffects.Copy : DragDropEffects.None;
        }
        private void JobMenuControl_DragDrop(object sender, DragEventArgs e)
        {
            var url = e.Data.GetData(DataFormats.Text).ToString();
            DownloadJob dJob;
            lock (_joblock)
            {
                dJob = new DownloadJob(url, job.getFileName());
            }
            var control = new DownloadJobControl(dJob);
            lock (_djoblock)
            {
                _djobs.Add(dJob);
            }
            dJob.DownloadFinished += (o, args) =>
            {
                int remjobs;
                lock (_djoblock)
                {
                    _djobs.Remove(o);
                    Invoke(new Action(() => control.Dispose()));
                    remjobs = _djobs.Count;
                }
                if (args.filePath != null)
                {
                    lock (_joblock)
                    {
                        job.AddImage(Path.GetFileName(args.filePath));
                    }
                    Invoke(new Action(refreshInfo));
                }
                if (remjobs == 0)
                {
                    lock (_joblock)
                    {
                        job.save();

                    }
                }
            };
            flowLayoutPanel1.Controls.Add(control);
        }
    }
}

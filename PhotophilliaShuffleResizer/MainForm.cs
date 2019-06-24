using System;
using System.Windows.Forms;

namespace PhotophilliaShuffleResizer
{
    public partial class MainForm : /*ReturnForm<Tuple<ResizingJob,string>>*/Form
    {
        public MainForm(string jobfile = null)
        {
            InitializeComponent();
            if (jobfile != null)
                selectJob(jobfile);
        }
        private void selectJob(string jobfile)
        {
            this.jobSelectControl1_OnJobSelected(this,new JobSelectedArgs(ResizingJob.load(jobfile)));
        }
        private void jobSelectControl1_OnJobSelected(object sender, JobSelectedArgs args)
        {
            jobSelectControl1.Visible = false;
            jobMenuControl1.Visible = true;
            jobMenuControl1.job = args.job;
        }

        private void jobMenuControl1_BackPressed(object sender, EventArgs e)
        {
            jobSelectControl1.Visible = true;
            jobMenuControl1.Visible = false;
        }
    }
}

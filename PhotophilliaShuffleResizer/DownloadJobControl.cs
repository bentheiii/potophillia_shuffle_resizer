using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhotophilliaShuffleResizer
{
    public partial class DownloadJobControl : UserControl
    {
        private readonly DownloadJob dJob;

        public DownloadJobControl(DownloadJob dJob)
        {
            this.dJob = dJob;
            InitializeComponent();
        }

        private void DownloadJobControl_Load(object sender, EventArgs e)
        {
            dJob.startDownload();
            button1.Enabled = true;
            label1.Text = dJob.getInfo();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dJob.AbortDownload();
        }
    }
}

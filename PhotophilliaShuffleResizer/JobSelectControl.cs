using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ookii.Dialogs;
using WhetStone.Streams;

namespace PhotophilliaShuffleResizer
{
    public delegate void JobSelectedHandler(object sender, JobSelectedArgs args);
    public partial class JobSelectControl : UserControl
    {
        public JobSelectControl()
        {
            InitializeComponent();
        }
        public event JobSelectedHandler JobSelected;

        private void CreateButton_Click(object sender, EventArgs e)
        {
            var diag = new VistaFolderBrowserDialog
            {
                Description = "Choose Raw Director",
                RootFolder = Environment.SpecialFolder.MyComputer,
                UseDescriptionForTitle = true,
                ShowNewFolderButton = false
            };
            if (diag.ShowDialog() == DialogResult.Cancel)
                return;
            var destlistpath = Path.Combine(diag.SelectedPath, "destinations.txt");
            if (!File.Exists(destlistpath))
            {
                MessageBox.Show("Destinations file not found!");
                return;
            }
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            string[] destlist;
            using (var streamReader = new StreamReader(destlistpath))
            {
                destlist = streamReader.Loop().ToArray();
            }
            var job = ResizingJob.Create(destlist, diag.SelectedPath, saveFileDialog1.FileName);
            JobSelected.Invoke(this,new JobSelectedArgs(job));
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            ResizingJob job = ResizingJob.load(openFileDialog1.FileName);
            JobSelected.Invoke(this, new JobSelectedArgs(job));
        }
    }
    public class JobSelectedArgs : EventArgs
    {
        public JobSelectedArgs(ResizingJob job)
        {
            this.job = job;
        }
        public ResizingJob job { get; }
    }
}

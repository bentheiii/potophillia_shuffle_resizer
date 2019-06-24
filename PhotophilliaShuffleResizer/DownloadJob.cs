using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WhetStone.SystemExtensions;

namespace PhotophilliaShuffleResizer
{
    public class DownloadJobFinishedEventArgs : EventArgs
    {
        public DownloadJobFinishedEventArgs(string filePath)
        {
            this.filePath = filePath;
        }
        public string filePath { get; }
    }
    public delegate void DownloadJobFinishedHandler(DownloadJob sender, DownloadJobFinishedEventArgs args);
    public class DownloadJob
    {
        private readonly string _url;
        private readonly string _destPath;
        private Thread _dThread = null;
        public event DownloadJobFinishedHandler DownloadFinished;
        public DownloadJob(string url, string destPath)
        {
            _url = url;
            _destPath = destPath;
        }
        public void startDownload()
        {
            ThreadStart act = () =>
            {
                string dpath = null;
                while (true)
                {
                    using (var c = new WebClient())
                    {
                        try
                        {
                            c.DownloadFile(_url, _destPath);
                            dpath = _destPath;
                        }
                        catch (ThreadAbortException){}
                        catch (Exception e)
                        {
                            var reply =
                                MessageBox.Show(
                                    "Couldn't download image from " + _url + ". Copy URL to clipboard? Press cancel to retry. Reason: " + e.Message,
                                    "failed download", MessageBoxButtons.YesNoCancel);
                            switch (reply)
                            {
                                case DialogResult.Yes:
                                    var t = new Thread(() => Clipboard.SetText(_url));
                                    t.SetApartmentState(ApartmentState.STA);
                                    t.Start();
                                    break;
                                case DialogResult.Cancel:
                                    continue;
                            }
                        }
                        break;
                    }
                }
                DownloadFinished.Invoke(this,new DownloadJobFinishedEventArgs(dpath));
            };
            _dThread = new Thread(act);
            _dThread.Start();
        }
        public void AbortDownload()
        {
            if (_dThread == null)
                throw new Exception("download hasn't started");
            _dThread.Abort();
        }
        public string getInfo()
        {
            return $"{_url.GetHashCode()}";
        }
        public override int GetHashCode()
        {
            return _url.GetHashCode() ^ _destPath.GetHashCode();
        }
    }
}

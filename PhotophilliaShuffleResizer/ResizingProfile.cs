using System.Collections.Generic;
using System.IO;
using System.Linq;
using WhetStone.Looping;
using WhetStone.Streams;

namespace PhotophilliaShuffleResizer
{
    public class ResizingProfile
    {
        public double maxZoom { get; }
        public double maxCropCoefficient { get; }
        /*autosizing is a as follows:
         * if zoom > maxZoom, set to native
         * if crop/zoom < maxcropcoefficient, set to crop
         * set to zoom
         */
        public ResizingProfile(int width, int height, int bottomgap, int topgap, double maxZoom, double maxCropCoefficient)
        {
            this.maxZoom = maxZoom;
            this.maxCropCoefficient = maxCropCoefficient;
            this.height = height;
            this.width = width;
            this.bottomgap = bottomgap;
            this.topgap = topgap;
        }
        public int height { get; }
        public int width { get; }
        public int bottomgap { get; }
        public int topgap { get; }
        public static ResizingProfile fromFile(string path)
        {
            IDictionary<string, double> data;
            if (!File.Exists(path))
                throw new FileNotFoundException();
            using (var streamReader = new StreamReader(path))
            {
                data = streamReader.Loop().Select(a =>
                {
                    var s = a.Split(':');
                    return new KeyValuePair<string, double>(s[0], double.Parse(s[1]));
                }).ToDictionary();
            }
            return new ResizingProfile((int)data["wi"], (int)data["he"], (int)data["gb"], (int)data["gt"], data["mz"], data["mcc"]);
        }
        public override string ToString()
        {
            return $"{height}, {width}, {bottomgap}";
        }
    }
}

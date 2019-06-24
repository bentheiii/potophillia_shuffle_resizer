using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CipherStone;
using WhetStone.Guard;
using WhetStone.Looping;
using WhetStone.Path;
using WhetStone.Streams;
using WhetStone.WordPlay;

namespace PhotophilliaShuffleResizer
{
    [Serializable] public class ResizingJob
    {
        public string rawdir { get; }
        private IDictionary<string,  Tuple<int,ResizingProfile>> _dests;
        private IDictionary<string, DialogResult?[]> _images;
        private int _imagesDone;
        public string sourcePath { get; set; }
        private int _emptyFileSpot = 0;
        private ResizingJob(string rawdir, IDictionary<string, Tuple<int, ResizingProfile>> dests, string sourcePath, int imagesDone, IDictionary<string, DialogResult?[]> images = null)
        {
            this.rawdir = rawdir;
            _dests = dests;
            this.sourcePath = sourcePath;
            this._imagesDone = imagesDone;
            _images = images ?? new Dictionary<string, DialogResult?[]>();
        }
        //returns the indexes to map every value in the original array to the new one. a -1 means the value should not be moved
        //example: returned array is [-1,3,1,-1], outpt size is 6
        //[1,2,3,4] -> [null,3,null,2,null,null]
        private static int[] adjustDestMap(IDictionary<string, int> original, IEnumerable<string> @new)
        {
            int[] ret = new int[original.Count];
            ret.Fill(-1);
            foreach (var tuple in @new.CountBind())
            {
                int ind;
                if (original.TryGetValue(tuple.Item1, out ind))
                    ret[ind] = tuple.Item2;
            }
            return ret;
        }
        public string getFileName()
        {
            string fullfilepath;
            while (true)
            {
                var filename = _emptyFileSpot + ".png";
                if (_images.ContainsKey(filename))
                {
                    _emptyFileSpot++;
                    continue;
                }
                fullfilepath = Path.Combine(rawdir, filename);
                if (File.Exists(fullfilepath))
                {
                    _emptyFileSpot++;
                    continue;
                }
                break;
            }
            return fullfilepath;
        }
        private static DialogResult?[] transform(DialogResult?[] original, int[] transform, int resultlength)
        {
            var ret = new DialogResult?[resultlength];
            IGuard<int> ind = new Guard<int>();
            foreach (var targ in transform.CountBind().Detach(ind).Where(a=>a>0))
            {
                ret[targ] = original[ind.value];
            }
            return ret;
        }
        public void rePoolDests()
        {
            var ndpath = Path.Combine(rawdir, "destinations.txt");
            string[] newdests;
            using (var reader = new StreamReader(ndpath))
            {
                newdests = reader.Loop().ToArray();
            }
            var transform = adjustDestMap(_dests.Select(x => x.Item1), newdests);

            foreach (var image in _images.Keys.ToArray())
            {
                _images[image] = ResizingJob.transform(_images[image], transform, newdests.Length);
            }
            _dests = newdests.Select((a, i) =>
            {
                var propath = Path.Combine(a, "preset.txt");
                return new KeyValuePair<string, Tuple<int, ResizingProfile>>(a, Tuple.Create(i, ResizingProfile.fromFile(propath)));
            }).ToDictionary();
        }
        public void rePoolImages(string[] validextensions)
        {
            string[] newfiles = validextensions.Select(a=>Directory.GetFiles(rawdir,a)).Concat().Select(a=>Path.GetFileName(a)).ToArray();
            IDictionary<string, DialogResult?[]> newdic = new Dictionary<string, DialogResult?[]>();

            foreach (var file in newfiles)
            {
                DialogResult?[] arr;
                if (!_images.TryGetValue(file, out arr))
                    arr = fill.Fill(_dests.Count, (DialogResult?)null);
                newdic[file] = poolImage(file, arr);
            }
            _images = newdic;
        }
        private DialogResult?[] poolImage(string file, DialogResult?[] arr)
        {
            var ret = arr.ToArray();
            foreach (KeyValuePair<string, int> pair in _dests.Select(a=>a.Item1))
            {
                string filepath = Path.Combine(pair.Key, file);
                bool exists = File.Exists(filepath);
                switch (arr[pair.Value])
                {
                    case null:
                        if (exists)
                            ret[pair.Value] = DialogResult.OK;
                        break;
                    case DialogResult.OK:
                        if (!exists)
                            ret[pair.Value] = null;
                        break;
                    case DialogResult.Abort:
                    case DialogResult.Ignore:
                        if (exists)
                            ret[pair.Value] = DialogResult.OK;
                        break;
                }
            }
            return ret;
        }
        public string info()
        {
            var total = countTotal();
            return $"{_imagesDone} out of {total} ({_imagesDone/(double)total:p2}) images done.";
        }
        public static ResizingJob Create(string[] destinations, string rawdir, string sourcePath)
        {
            var dests = new Dictionary<string, Tuple<int, ResizingProfile>>(destinations.Length);
            int i = 0;
            foreach (var destination in destinations)
            {
                var propath = Path.Combine(destination, "preset.txt");
                dests[destination] = Tuple.Create(i++, ResizingProfile.fromFile(propath));
            }
            return new ResizingJob(rawdir,dests, sourcePath,0);
        }
        public DialogResult? get(string dest, string image)
        {
            DialogResult?[] arr;
            if (!_images.TryGetValue(image, out arr))
                return null;
            Tuple<int, ResizingProfile> indTuple;
            if (!_dests.TryGetValue(dest, out indTuple))
                throw new Exception("destoination not found!");
            return arr[indTuple.Item1];
        }
        public void set(string dest, string image, DialogResult val)
        {
            DialogResult?[] arr;
            if (!_images.TryGetValue(image, out arr))
            {
                arr = new DialogResult?[_dests.Count];
                _images.Add(image,arr);
            }
            Tuple<int, ResizingProfile> indTuple;
            if (!_dests.TryGetValue(dest, out indTuple))
                throw new Exception("destoination not found!");
            if (arr[indTuple.Item1] == null)
                this._imagesDone++;
            arr[indTuple.Item1] = val;
        }
        public int countTotal()
        {
            return _dests.Count * _images.Count;
        }
        public Tuple<string,ResizingProfile>[] destinations
        {
            get
            {
                return _dests.OrderBy(x=>x.Value.Item1).Select(a=>Tuple.Create(a.Key,a.Value.Item2)).ToArray();
            }
        }
        public static string AbsolutePathToRelative(string origin, string destination)
        {
            if (string.IsNullOrEmpty(origin))
                throw new ArgumentException("Value cannot be null or empty.", nameof(origin));
            if (string.IsNullOrEmpty(destination))
                throw new ArgumentException("Value cannot be null or empty.", nameof(destination));
            if (origin.Substring(1, 2) != @":\")
                throw new Exception("argument is not an absolute path!");
            if (destination.Substring(1, 2) != @":\")
                throw new Exception("argument is not an absolute path!");
            origin = origin.Remove(1, 1);
            destination = destination.Remove(1, 1);
            IList<string> osplit = origin.Split(new[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
            IList<string> dsplit = destination.Split(new[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
            while (osplit[0].Equals(dsplit[0]))
            {
                osplit = osplit.Skip(1);
                dsplit = dsplit.Skip(1);
                if (!(osplit.Any() && dsplit.Any()))
                    break;
            }
            IList<string> ret = osplit.Select(a => "..").Concat(dsplit);
            return ret.StrConcat("\\");
        }
        //NEW
        //first line: start with <<, after that the version, after that has the raw directory path
        //next lines: start with >, after that destination path
        //every line after that is an image file name, and results, seperated by ':'. a - means the image has not been processed
        //OLD, also supported
        //encoding:
        //first line: start with ::, after that has the raw directory path
        //start a dest with <, continue with dest path (on new line)
        //everything after dest path is of format name>res. name of image and dialog result (in decimal int form)
        private IEnumerable<string> encode(IComparer<KeyValuePair<string, DialogResult?[]>> sortImages = null)
        {
            yield return "<<2" + rawdir;
            foreach (var dest in _dests.OrderBy(a=>a.Value.Item1).Select(a=>a.Key))
            {
                var rel = AbsolutePathToRelative(rawdir, dest);
                yield return ">" + rel;
            }
            var im = sortImages == null ? (IEnumerable<KeyValuePair<string, DialogResult?[]>>)_images : _images.OrderBy(sortImages);
            foreach (KeyValuePair<string, DialogResult?[]> image in im)
            {
                StringBuilder line = new StringBuilder(image.Key+":",image.Key.Length+_dests.Count*2);
                line.Append(image.Value.Select(a => a.HasValue ? ((int)a).ToString() : "-").StrConcat(":"));
                yield return line.ToString();
            }
        }
        private static ResizingJob Decode(IEnumerable<string> lines, string sourcePath)
        {
            var header = lines.FirstOrDefault("");
            if (header.StartsWith("<<2"))
                return decode2(lines, sourcePath);
            throw new Exception("can't read buffer");
        }
        private static ResizingJob decode2(IEnumerable<string> lines, string sourcePath)
        {
            using (var tor = lines.GetEnumerator())
            {
                int done = 0;
                var dests = new Dictionary<string, Tuple<int, ResizingProfile>>();
                var images = new Dictionary<string, DialogResult?[]>();
                if (!tor.MoveNext() || !tor.Current.StartsWith("<<"))
                    throw new Exception("bad initial line");
                var raw = tor.Current.Substring("<<2".Length);
                int ind = 0;
                while (true)
                {
                    if (!tor.MoveNext())
                        return new ResizingJob(raw, dests, sourcePath, 0, images);
                    if (!tor.Current.StartsWith(">"))
                        break;
                    var reldestpath = tor.Current.Substring(1);
                    var destpath = Path.GetFullPath(Path.Combine(raw, reldestpath));
                    var propath = Path.Combine(destpath, "preset.txt");
                    var profile = ResizingProfile.fromFile(propath);
                    dests.Add(destpath,new Tuple<int, ResizingProfile>(ind++,profile));
                }
                while (true)
                {
                    var split = tor.Current.Split(':');
                    if (split.Length != dests.Count + 1)
                        throw new Exception("argument mismatch:" + tor.Current);
                    var args = split.Skip(1).Select(a=>
                    {
                        if (a == "-")
                            return null;
                        done++;
                        return (DialogResult?)(DialogResult)int.Parse(a);
                    }).ToArray();
                    images.Add(split[0],args);
                    if (!tor.MoveNext())
                        return new ResizingJob(raw, dests, sourcePath, done, images);
                }
            }
        }
        public void save()
        {
            if (sourcePath.EndsWith(".psj"))
                saveStandard();
            else if (sourcePath.EndsWith(".pzj"))
                saveZipped();
            else
                throw new Exception("Extention not recognized");
        }
        private void saveStandard()
        {
            using (var writer = new StreamWriter(sourcePath))
            {
                foreach (var line in encode())
                {
                    writer.WriteLine(line);
                }
            }
        }
        private void saveZipped()
        {
            var plaintext = encode().StrConcat("|");
            var plainbytes = Encoding.UTF8.GetBytes(plaintext);
            File.WriteAllBytes(sourcePath,plainbytes.Compress());
        }
        public static ResizingJob load(string path)
        {
            if (path.EndsWith(".psj"))
                return loadStandard(path);
            if (path.EndsWith(".pzj"))
                return loadZipped(path);
            throw new Exception("Extention not recognized");
        }
        private static ResizingJob loadZipped(string path)
        {
            var cipherbytes = File.ReadAllBytes(path);
            var decodedbytes = cipherbytes.Decompress();
            var decodedtext = Encoding.UTF8.GetString(decodedbytes);
            return Decode(decodedtext.AsEnumerable().Split('|').Select(a=>a.ConvertToString()), path);
        }
        private static ResizingJob loadStandard(string path)
        {
            using (var reader = new StreamReader(path))
            {
                return Decode(reader.Loop(cache:3), path);
            }
        }
        public IList<Tuple<string, string>> getPairs()
        {
            var ret = new List<Tuple<string,string>>();
            var dests = destinations.Select(a=>a.Item1);
            foreach (var image in _images)
            {
                ret.AddRange(dests.CountBind().Where(a => image.Value[a.Item2] == null).Detach().Select(dest => Tuple.Create(image.Key, dest)));
            }
            return ret;
        }
        public ResizingProfile getProfile(string dest)
        {
            return _dests[dest].Item2;
        }
        public void AddImage(string name)
        {
            _images.Add(name, fill.Fill(_dests.Count, (DialogResult?)null));
        }
    }
}

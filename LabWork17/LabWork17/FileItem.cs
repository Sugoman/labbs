using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabWork17
{
    internal class FileItem
    {
        public string Path { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public DateTime DateModified { get; set; }
        public string Icon { get; set; }

        public FileItem(string path)
        {
            Path = path;
            Name = System.IO.Path.GetFileName(path);
            Type = System.IO.Path.GetExtension(path).ToUpper();
            DateModified = File.GetLastWriteTime(path);
        }
    }
}

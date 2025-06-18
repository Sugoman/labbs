using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabWork17
{
    using System.Collections.ObjectModel;
    using System.IO;
    internal class Folder
    {
        public string Path { get; set; }
        public string Name { get; set; }
        public ObservableCollection<Folder> SubFolders { get; set; } = new();
        public ObservableCollection<FileItem> Files { get; set; } = new();
        public string Icon { get; set; }

        public Folder(string path)
        {
            Path = path;
            Name = new DirectoryInfo(path).Name;
        }
    }
}

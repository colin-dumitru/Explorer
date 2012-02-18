using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBLibrary {
    public class MyFile {
        public int Id { get; set; }
        public string Name { get; set; }
        public int IsDirectory { get; set; }
        public int Parent { get; set; }

        public MyFile(int id, string name, int isdir, int parent)
        {
            this.Id = id;
            this.Name = name;
            this.Parent = parent;
            this.IsDirectory = isdir;
        }
    }
}

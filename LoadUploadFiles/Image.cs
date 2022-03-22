using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadUploadFiles
{
    internal class Image
    {
        public int Id { get; private set; }
        public string FileName { get; private set; }
        public string Title { get; private set; }
        public byte[] Data { get; private set; }

        public Image(int id, string fileName, string title, byte[] data)
        {
            Id = id;
            FileName = fileName;
            Title = title;
            Data = data;
        }
    }
}

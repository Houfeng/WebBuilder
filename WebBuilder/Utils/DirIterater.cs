using System.IO;

namespace WebBuilder.Utils
{
    public delegate void FoundDirectoryHandler(DirectoryInfo dir);
    public delegate void FoundFileHandler(FileInfo file);
    public class DirIterater
    {
        public DirectoryInfo Root { get; set; }
        public DirIterater(string root)
        {
            this.Root = Directory.CreateDirectory(root);
        }
        public DirIterater(DirectoryInfo root)
        {
            this.Root = root;
        }
        public event FoundDirectoryHandler FoundDirectory;
        public event FoundFileHandler FoundFile;
        private void OnFoundDirectory(DirectoryInfo dir)
        {
            if (this.FoundDirectory != null)
                this.FoundDirectory(dir);
        }
        private void OnFoundFile(FileInfo file)
        {
            if (this.FoundFile != null)
                this.FoundFile(file);
        }
        private void Handle(DirectoryInfo dir)
        {
            this.OnFoundDirectory(dir);
            //处理子目录
            DirectoryInfo[] childDirs = dir.GetDirectories();
            foreach (DirectoryInfo childDir in childDirs)
            {
                this.Handle(childDir);
            }
            //处理文件
            FileInfo[] childFiles = dir.GetFiles();
            foreach (FileInfo childFile in childFiles)
            {
                this.OnFoundFile(childFile);
            }
        }
        public void Start()
        {
            this.Handle(this.Root);
        }
    }
}

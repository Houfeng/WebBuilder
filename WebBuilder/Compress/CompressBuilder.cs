using System;
using System.Collections.Generic;
using System.IO;
using WebBuilder.Utils;

namespace WebBuilder.Compress
{
    public class CompressBuilder
    {
        private Dictionary<string, CompressorBase> Compressors { get; set; }
        private string Separator { get; set; }
        private CmdParameter cmdParameter { get; set; }
        public CompressBuilder(CmdParameter cmdParameter)
        {
            this.cmdParameter = cmdParameter;
            this.Separator = this.cmdParameter.platform == "windows" ? "\\" : "/";
            this.cmdParameter.inDir = this.cmdParameter.inDir.Replace("\\", this.Separator).Replace("/", this.Separator);
            this.cmdParameter.outDir = this.cmdParameter.outDir.Replace("\\", this.Separator).Replace("/", this.Separator);
            this.Compressors = new Dictionary<string, CompressorBase>();
            this.Compressors.Add(".*", new GeneralCompressor(this.cmdParameter));
            this.Compressors.Add(".js", new JsCompressor(this.cmdParameter));
            this.Compressors.Add(".css", new CssCompressor(this.cmdParameter));
        }
        private bool canHandle(string filePath)
        {
            if (this.cmdParameter.ignoreList == null)
            {
                return true;
            }
            foreach (var _path in this.cmdParameter.ignoreList)
            {
                var path = string.Format("{0}{1}{2}", this.cmdParameter.inDir, this.Separator, _path);
                path = path.Replace("\\", this.Separator).Replace("/", this.Separator);
                filePath = filePath.Replace("\\", this.Separator).Replace("/", this.Separator);
                path = path.Replace(this.Separator + this.Separator, this.Separator);
                if (filePath.ToLower().StartsWith(path.ToLower()))
                {
                    return false;
                }
            }
            return true;
        }
        private void Handle(DirectoryInfo inDir, DirectoryInfo outDir)
        {
            outDir.Create();
            //处理子目录
            DirectoryInfo[] childInDirs = inDir.GetDirectories();
            foreach (DirectoryInfo childInDir in childInDirs)
            {
#if !DEBUG
                try
                {
#endif
                    DirectoryInfo childOutDir = Directory.CreateDirectory(string.Format("{0}{1}{2}", outDir.FullName, this.Separator, childInDir.Name));
                    this.Handle(childInDir, childOutDir);
                    Console.WriteLine(string.Format("folder:\"{0}\".", childInDir.FullName));
#if !DEBUG
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("folder:\"{0}\",error:\"{1}\".", childInDir.FullName, ex.Message));
                }
#endif
            }
            //处理文件
            FileInfo[] childInFiles = inDir.GetFiles();
            foreach (FileInfo childInFile in childInFiles)
            {
#if !DEBUG
                try
                {
#endif
                    var exName = Path.GetExtension(childInFile.Name);
                    CompressorBase compressor = this.Compressors[".*"];
                    if (this.Compressors.ContainsKey(exName))
                    {
                        compressor = this.Compressors[exName];
                    }
                    byte[] content = File.ReadAllBytes(childInFile.FullName);
                    if (this.canHandle(childInFile.FullName))
                    {
                        content = compressor.Compress(content);
                    }
                    File.WriteAllBytes(string.Format("{0}{1}{2}", outDir.FullName, this.Separator, childInFile.Name), content);
                    Console.WriteLine(string.Format("file:\"{0}\".", childInFile.FullName));
#if !DEBUG
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("file:\"{0}\",error:\"{1}\".", childInFile.FullName, ex.Message));
                }
#endif
            }
        }
        public void Excute()
        {
            Console.WriteLine("Compress Start.");
            DirectoryInfo inDir = Directory.CreateDirectory(this.cmdParameter.inDir);
            DirectoryInfo outDir = Directory.CreateDirectory(this.cmdParameter.outDir);
            this.Handle(inDir, outDir);
            Console.WriteLine("Compress Done.");
        }
    }
}


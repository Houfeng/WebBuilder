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
            this.Compressors = new Dictionary<string, CompressorBase>();
            this.Compressors.Add(".*", new GeneralCompressor(this.cmdParameter));
            this.Compressors.Add(".js", new JsCompressor(this.cmdParameter));
            this.Compressors.Add(".css", new CssCompressor(this.cmdParameter));
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
                    byte[] content = compressor.Compress(File.ReadAllBytes(childInFile.FullName));
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


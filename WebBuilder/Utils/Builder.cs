using System;
using System.Collections.Generic;
using System.IO;

namespace WebBuilder.Utils
{
    public class Builder
    {
        private Dictionary<string, CompressorBase> Compressors { get; set; }
        private string Separator { get; set; }
        private Parameter Parameter { get; set; }
        public Builder(Parameter parameter)
        {
            this.Parameter = parameter;
            this.Separator = this.Parameter.platform == "windows" ? "\\" : "/";
            this.Compressors = new Dictionary<string, CompressorBase>();
            this.Compressors.Add(".*", new GeneralCompressor(this.Parameter));
            this.Compressors.Add(".js", new JsCompressor(this.Parameter));
            this.Compressors.Add(".css", new CssCompressor(this.Parameter));
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
            Console.WriteLine("start.");
            DirectoryInfo inDir = Directory.CreateDirectory(this.Parameter.inDir);
            DirectoryInfo outDir = Directory.CreateDirectory(this.Parameter.outDir);
            this.Handle(inDir, outDir);
            Console.WriteLine("done.");
        }
    }
}


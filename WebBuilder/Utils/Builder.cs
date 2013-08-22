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
            this.Separator = this.Parameter.platform == "window" ? "\\" : "/";
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
                try
                {
                    DirectoryInfo childOutDir = Directory.CreateDirectory(string.Format("{0}{1}{2}", outDir.FullName, this.Separator, childInDir.Name));
                    this.Handle(childInDir, childOutDir);
                    Console.WriteLine(string.Format("处理目录'{0}'成功.", childInDir.FullName));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("处理目录'{0}'时,发生'{1}'错误.", childInDir.FullName, ex.Message));
                }
            }
            //处理文件
            FileInfo[] childInFiles = inDir.GetFiles();
            foreach (FileInfo childInFile in childInFiles)
            {
                try
                {
                    var exName = Path.GetExtension(childInFile.Name);
                    CompressorBase compressor = this.Compressors[".*"];
                    if (this.Compressors.ContainsKey(exName))
                    {
                        compressor = this.Compressors[exName];
                    }
                    byte[] content = compressor.Compress(File.ReadAllBytes(childInFile.FullName));
                    File.WriteAllBytes(string.Format("{0}{1}{2}", outDir.FullName, this.Separator, childInFile.Name), content);
                    Console.WriteLine(string.Format("处理文件'{0}'成功.", childInFile.FullName));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("处理文件'{0}'时,发生'{1}'错误.", childInFile.FullName, ex.Message));
                }
            }
        }
        public void Excute()
        {
            Console.WriteLine("WebBuilder Powered By Houfeng");
            DirectoryInfo inDir = Directory.CreateDirectory(this.Parameter.inDir);
            DirectoryInfo outDir = Directory.CreateDirectory(this.Parameter.outDir);
            this.Handle(inDir, outDir);
            Console.WriteLine("处理完成.");
        }
    }
}


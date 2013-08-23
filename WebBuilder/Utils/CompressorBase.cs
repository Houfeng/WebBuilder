using System.IO;
using System.IO.Compression;

namespace WebBuilder.Utils
{
    public abstract class CompressorBase
    {
        public Parameter Parameter { get; set; }
        public CompressorBase(Parameter parameter)
        {
            this.Parameter = parameter;
        }
        private byte[] ToGzip(byte[] source)
        {
            MemoryStream outStream = new MemoryStream();
            MemoryStream inStream = new MemoryStream(source);
            GZipStream zipStream = new GZipStream(outStream, CompressionMode.Compress);
            inStream.WriteTo(zipStream);
            zipStream.Close();
            outStream.Close();
            inStream.Close();
            return outStream.ToArray();
        }
        public virtual byte[] Compress(byte[] source)
        {
            return source;
        }
    }
}

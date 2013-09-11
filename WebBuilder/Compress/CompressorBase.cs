using System.IO;
using System.IO.Compression;
using WebBuilder.Utils;

namespace WebBuilder.Compress
{
    public abstract class CompressorBase
    {
        public CmdParameter CmdParameter { get; set; }
        public CompressorBase(CmdParameter cmdParameter)
        {
            this.CmdParameter = cmdParameter;
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

using System.Text;
using Yahoo.Yui.Compressor;
using yui = Yahoo.Yui.Compressor;

namespace WebBuilder.Utils
{
    public class CssCompressor : CompressorBase
    {
        private yui.CssCompressor cssCompressor { get; set; }
        public CssCompressor(Parameter parameter)
            : base(parameter)
        {
            this.cssCompressor = new yui.CssCompressor();
            this.cssCompressor.CompressionType = CompressionType.Standard;
            this.cssCompressor.LineBreakPosition = parameter.lineBreak < 1 ? int.MaxValue : parameter.lineBreak;
            this.cssCompressor.RemoveComments = parameter.removeComments;
        }
        public override byte[] Compress(byte[] source)
        {
            if (source.Length < 1) return source;
            var srcText = Encoding.UTF8.GetString(source);
            var dstText = this.cssCompressor.Compress(srcText);
            return Encoding.UTF8.GetBytes(dstText);
        }
    }
}

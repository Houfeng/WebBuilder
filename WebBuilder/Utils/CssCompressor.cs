using System.Text;
using Yahoo.Yui.Compressor;
using yui = Yahoo.Yui.Compressor;

namespace WebBuilder.Utils
{
    public class CssCompressor : CompressorBase
    {
        private const string CompressedComment = "/*csd*/";
        private yui.CssCompressor InnerCompressor { get; set; }
        private Encoding Encoding { get; set; }
        public CssCompressor(Parameter parameter)
            : base(parameter)
        {
            this.InnerCompressor = new yui.CssCompressor();
            this.InnerCompressor.CompressionType = CompressionType.Standard;
            this.InnerCompressor.LineBreakPosition = parameter.lineBreak < 1 ? int.MaxValue : parameter.lineBreak;
            this.InnerCompressor.RemoveComments = parameter.removeComments;
            this.Encoding = Encoding.GetEncoding(string.IsNullOrEmpty(parameter.encoding) ? "UTF-8" : parameter.encoding);
        }
        public override byte[] Compress(byte[] source)
        {
            if (source.Length < 1) return source;
            var srcText = this.Encoding.GetString(source);
            if (srcText.StartsWith(CompressedComment))
            {
                return base.Compress(this.Encoding.GetBytes(srcText));
            }
            else
            {
                var dstText = string.Format("{0}{1}", this.Parameter.addMark ? CompressedComment : "", this.InnerCompressor.Compress(srcText));
                return base.Compress(this.Encoding.GetBytes(dstText));
            }
        }
    }
}

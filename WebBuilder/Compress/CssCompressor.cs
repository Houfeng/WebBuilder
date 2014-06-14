using dotless.Core;
using System.Text;
using WebBuilder.Utils;
using Yahoo.Yui.Compressor;
using yui = Yahoo.Yui.Compressor;

namespace WebBuilder.Compress
{
    public class CssCompressor : CompressorBase
    {
        private const string CompressedComment = "/*csd*/";
        private yui.CssCompressor InnerCompressor { get; set; }
        private Encoding Encoding { get; set; }
        public CssCompressor(CmdParameter cmdParameter)
            : base(cmdParameter)
        {
            this.InnerCompressor = new yui.CssCompressor();
            this.InnerCompressor.CompressionType = CompressionType.Standard;
            this.InnerCompressor.LineBreakPosition = cmdParameter.lineBreak < 1 ? int.MaxValue : cmdParameter.lineBreak;
            this.InnerCompressor.RemoveComments = cmdParameter.removeComments;
            this.Encoding = Encoding.GetEncoding(string.IsNullOrEmpty(cmdParameter.encoding) ? "UTF-8" : cmdParameter.encoding);
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
                srcText = Less.Parse(srcText);
                var dstText = string.Format("{0}{1}", this.CmdParameter.addMark ? CompressedComment : "", this.InnerCompressor.Compress(srcText));
                return base.Compress(this.Encoding.GetBytes(dstText));
            }
        }
    }
}

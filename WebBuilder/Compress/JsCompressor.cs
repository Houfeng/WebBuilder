using System.Text;
using WebBuilder.Utils;
using Yahoo.Yui.Compressor;

namespace WebBuilder.Compress
{
    public class JsCompressor : CompressorBase
    {
        private const string CompressedComment = "/*csd*/";
        private JavaScriptCompressor InnerCompressor { get; set; }
        private Encoding Encoding { get; set; }
        public JsCompressor(CmdParameter cmdParameter)
            : base(cmdParameter)
        {
            this.InnerCompressor = new JavaScriptCompressor();
            this.InnerCompressor.CompressionType = CompressionType.Standard;
            this.InnerCompressor.DisableOptimizations = cmdParameter.optimize;
            this.InnerCompressor.ObfuscateJavascript = cmdParameter.obfuscate;
            this.Encoding = Encoding.GetEncoding(string.IsNullOrEmpty(cmdParameter.encoding) ? "UTF-8" : cmdParameter.encoding);
            this.InnerCompressor.Encoding = this.Encoding;
            this.InnerCompressor.LineBreakPosition = cmdParameter.lineBreak < 1 ? int.MaxValue : cmdParameter.lineBreak;
            this.InnerCompressor.PreserveAllSemicolons = true;
            this.InnerCompressor.IgnoreEval = cmdParameter.ignoreEval;
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
                var dstText = string.Format("{0}{1}", this.CmdParameter.addMark ? CompressedComment : "", this.InnerCompressor.Compress(srcText));
                return base.Compress(this.Encoding.GetBytes(dstText));
            }
        }
    }
}

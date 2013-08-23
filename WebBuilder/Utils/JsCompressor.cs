using System.Text;
using Yahoo.Yui.Compressor;

namespace WebBuilder.Utils
{
    public class JsCompressor : CompressorBase
    {
        private const string CompressedComment = "/*csd*/";
        private JavaScriptCompressor InnerCompressor { get; set; }
        private Encoding Encoding { get; set; }
        public JsCompressor(Parameter parameter)
            : base(parameter)
        {
            this.InnerCompressor = new JavaScriptCompressor();
            this.InnerCompressor.CompressionType = CompressionType.Standard;
            this.InnerCompressor.DisableOptimizations = parameter.optimize;
            this.InnerCompressor.ObfuscateJavascript = parameter.obfuscate;
            this.Encoding = Encoding.GetEncoding(string.IsNullOrEmpty(parameter.encoding) ? "UTF-8" : parameter.encoding);
            this.InnerCompressor.Encoding = this.Encoding;
            this.InnerCompressor.LineBreakPosition = parameter.lineBreak < 1 ? int.MaxValue : parameter.lineBreak;
            this.InnerCompressor.PreserveAllSemicolons = true;
            this.InnerCompressor.IgnoreEval = parameter.ignoreEval;
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

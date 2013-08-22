using System;
using System.Text;
using Yahoo.Yui.Compressor;

namespace WebBuilder.Utils
{
    public class JsCompressor : CompressorBase
    {
        private JavaScriptCompressor jsCompressor { get; set; }
        public JsCompressor(Parameter parameter)
            : base(parameter)
        {
            this.jsCompressor = new JavaScriptCompressor();
            this.jsCompressor.CompressionType = CompressionType.Standard;
            this.jsCompressor.DisableOptimizations = parameter.optimize;
            this.jsCompressor.ObfuscateJavascript = parameter.obfuscate;
            this.jsCompressor.Encoding = Encoding.GetEncoding(string.IsNullOrEmpty(parameter.encoding) ? "UTF-8" : parameter.encoding);
            this.jsCompressor.LineBreakPosition = parameter.lineBreak < 1 ? int.MaxValue : parameter.lineBreak;
            this.jsCompressor.PreserveAllSemicolons = true;
            this.jsCompressor.IgnoreEval = parameter.ignoreEval;
        }
        public override byte[] Compress(byte[] source)
        {
            if (source.Length < 1) return source;
            var srcText = Encoding.UTF8.GetString(source);
            var dstText = this.jsCompressor.Compress(srcText);
            return Encoding.UTF8.GetBytes(dstText);
        }
    }
}

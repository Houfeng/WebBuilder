using WebBuilder.Utils;
namespace WebBuilder.Compress
{
    public class GeneralCompressor : CompressorBase
    {
        public GeneralCompressor(CmdParameter cmdParameter)
            : base(cmdParameter)
        {
        }
        public override byte[] Compress(byte[] source)
        {
            return base.Compress(source);
        }
    }
}

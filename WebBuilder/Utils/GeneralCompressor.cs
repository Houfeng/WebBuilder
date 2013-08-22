
namespace WebBuilder.Utils
{
    public class GeneralCompressor : CompressorBase
    {
        public GeneralCompressor(Parameter parameter)
            : base(parameter)
        {
        }
        public override byte[] Compress(byte[] source)
        {
            return base.Compress(source);
        }
    }
}

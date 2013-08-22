
namespace WebBuilder.Utils
{
    public abstract class CompressorBase
    {
        private Parameter Parameter { get; set; }
        public CompressorBase(Parameter parameter)
        {
            this.Parameter = parameter;
        }
        public virtual byte[] Compress(byte[] source)
        {
            return source;
        }
    }
}

using IB.Evaluation.Tokenizers.Base;
using IB.Evaluation.Tokenizers.Selectors.Base;

namespace IB.Evaluation.Tokenizers
{
    public class ArabicNumbersTokenizer : Tokenizer<double>
    {
        public ArabicNumbersTokenizer(TextReader reader, ITokenSelector selectors) : base(reader, selectors)
        {
        }
    }
}

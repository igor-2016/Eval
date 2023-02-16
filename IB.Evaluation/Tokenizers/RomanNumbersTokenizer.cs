using IB.Evaluation.Tokenizers.Base;
using IB.Evaluation.Tokenizers.Selectors.Base;

namespace IB.Evaluation.Tokenizers
{
    public class RomanNumbersTokenizer : Tokenizer<string>
    {
        public RomanNumbersTokenizer(TextReader reader, ITokenSelector selectors) : base(reader, selectors)
        {
        }
    }
}

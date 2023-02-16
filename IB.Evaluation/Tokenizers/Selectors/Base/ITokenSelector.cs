using IB.Evaluation.Tokenizers.Base;
using IB.Evaluation.Tokenizers.Enums;

namespace IB.Evaluation.Tokenizers.Selectors.Base
{
    public interface ITokenSelector
    {
        ITokenSelector SetNext(ITokenSelector selector);

        TokenTypes? Select(ITokenizer tokenizer, char currentChar);
    }

}

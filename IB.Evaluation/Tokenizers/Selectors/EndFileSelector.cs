using IB.Evaluation.Tokenizers.Base;
using IB.Evaluation.Tokenizers.Enums;
using IB.Evaluation.Tokenizers.Selectors.Base;

namespace IB.Evaluation.Tokenizers.Selectors
{
    public class EndFileSelector : TokenTypeSelectorBase
    {
        public override TokenTypes? Select(ITokenizer tokenizer, char currentChar)
        {
            if (currentChar == '\0')
            {
                return TokenTypes.EndOfFile;
            }
            else
            {
                return base.Select(tokenizer, currentChar);
            }
        }
    }

}

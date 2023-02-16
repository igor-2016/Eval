using IB.Evaluation.Tokenizers.Base;
using IB.Evaluation.Tokenizers.Enums;
using IB.Evaluation.Tokenizers.Selectors.Base;

namespace IB.Evaluation.Tokenizers.Selectors
{
    public class WhiteSpaceSelector : TokenTypeSelectorBase
    {
        public override TokenTypes? Select(ITokenizer tokenizer, char currentChar)
        {
            while (char.IsWhiteSpace(currentChar))
            {
                currentChar = tokenizer.MoveNextChar();
            }

            return base.Select(tokenizer, currentChar);
        }
    }

}

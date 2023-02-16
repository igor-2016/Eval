using IB.Evaluation.Tokenizers.Base;
using IB.Evaluation.Tokenizers.Enums;
using IB.Evaluation.Tokenizers.Selectors.Base;

namespace IB.Evaluation.Tokenizers.Selectors
{
    public class MathOpertionsSelector : TokenTypeSelectorBase
    {
        public override TokenTypes? Select(ITokenizer tokenizer, char currentChar)
        {
            if (currentChar == '*')
            {
                tokenizer.MoveNextChar();
                return TokenTypes.Multiplication;
            }
            else if (currentChar == '+')
            {
                tokenizer.MoveNextChar();
                return TokenTypes.Addition;
            }
            else if (currentChar == '-')
            {
                tokenizer.MoveNextChar();
                return TokenTypes.Substraction;
            }
            else if (currentChar == '/')
            {
                tokenizer.MoveNextChar();
                return TokenTypes.Division;
            }
            else
            {
                return base.Select(tokenizer, currentChar);
            }
        }
    }

}

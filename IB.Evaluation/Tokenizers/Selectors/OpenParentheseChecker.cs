using IB.Evaluation.Tokenizers.Base;
using IB.Evaluation.Tokenizers.Enums;
using IB.Evaluation.Tokenizers.Exceptions;
using IB.Evaluation.Tokenizers.Selectors.Base;

namespace IB.Evaluation.Tokenizers.Selectors
{
    public class OpenParentheseChecker : TokenTypeSelectorBase
    {
        protected Stack<char> parentheseChecker;

        public OpenParentheseChecker(Stack<char> checker)
        {
            parentheseChecker = checker;
        }

        public override TokenTypes? Select(ITokenizer tokenizer, char currentChar)
        {
            if (parentheseChecker.Count > 0)
                throw new ParentheseException($"Found: {parentheseChecker.Peek()}");
            else
                return base.Select(tokenizer, currentChar);
        }
    }

}

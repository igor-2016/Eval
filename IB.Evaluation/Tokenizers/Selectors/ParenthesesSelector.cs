using IB.Evaluation.Tokenizers.Base;
using IB.Evaluation.Tokenizers.Enums;
using IB.Evaluation.Tokenizers.Exceptions;
using IB.Evaluation.Tokenizers.Selectors.Base;

namespace IB.Evaluation.Tokenizers.Selectors
{
    public class ParenthesesSelector : TokenTypeSelectorBase
    {
        protected Stack<char> parentheseChecker;
        public ParenthesesSelector(Stack<char> checker)
        {
            parentheseChecker = checker;
        }

        public override TokenTypes? Select(ITokenizer tokenizer, char currentChar)
        {
            if (currentChar == '(')
            {
                if (parentheseChecker.Count == 0 || parentheseChecker.Peek() == '(')
                    parentheseChecker.Push('(');
                else if (parentheseChecker.Peek() == ')')
                    parentheseChecker.Pop();
                else
                    throw new ParentheseException($"Current: {currentChar}, found: {parentheseChecker.Peek()}");

                tokenizer.MoveNextChar();
                return TokenTypes.OpenParenthese;
            }
            else if (currentChar == ')')
            {
                if (parentheseChecker.Count == 0)
                    throw new ParentheseException($"No starting for {currentChar}");
                else if (parentheseChecker.Peek() == '(')
                    parentheseChecker.Pop();
                else
                    throw new ParentheseException($"Current: {currentChar}, found: {parentheseChecker.Peek()}");

                tokenizer.MoveNextChar();
                return TokenTypes.CloseParenthese;
            }
            else
            {
                return base.Select(tokenizer, currentChar);
            }
        }
    }

}

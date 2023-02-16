using IB.Evaluation.Calculators;
using IB.Evaluation.Tokenizers.Base;
using IB.Evaluation.Tokenizers.Enums;
using IB.Evaluation.Tokenizers.Selectors.Base;
using System.Globalization;
using System.Text;

namespace IB.Evaluation.Tokenizers.Selectors
{
    public class RomanNumbersSelector : TokenTypeSelectorBase
    {
        public static HashSet<char> Romans = new HashSet<char> { 'I', 'V', 'X', 'L', 'C', 'D', 'M' };
        public override TokenTypes? Select(ITokenizer tokenizer, char currentChar)
        {
            if (Romans.TryGetValue(currentChar, out char r))
            {
                var sb = new StringBuilder();
                while (Romans.TryGetValue(currentChar, out char ra))
                {
                    sb.Append(currentChar);
                    currentChar = tokenizer.MoveNextChar();
                }

                var number = sb.ToString();
               
                if (tokenizer is Tokenizer<string> t)
                {
                    t.Number = number.ToString();
                }
                return TokenTypes.Number;
            }
            else
            {
                return base.Select(tokenizer, currentChar);
            }
        }
    }

}

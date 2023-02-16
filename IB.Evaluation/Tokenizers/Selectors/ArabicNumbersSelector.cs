using System.Globalization;
using System.Text;
using IB.Evaluation.Calculators.Settings;
using IB.Evaluation.Tokenizers.Base;
using IB.Evaluation.Tokenizers.Enums;
using IB.Evaluation.Tokenizers.Selectors.Base;

namespace IB.Evaluation.Tokenizers.Selectors
{
    public class ArabicNumbersSelector : TokenTypeSelectorBase
    {
        ArabicCalcSettings Settings;
        public ArabicNumbersSelector(ArabicCalcSettings settings)
        {
            Settings = settings;
        }
        public override TokenTypes? Select(ITokenizer tokenizer, char currentChar)
        {
            if (char.IsDigit(currentChar) || currentChar == Settings.DecimalSeparator)
            {
                var sb = new StringBuilder();
                bool haveDecimalPoint = false;
                // TODO Settings.NumberGroup!
                while (char.IsDigit(currentChar) || !haveDecimalPoint && currentChar == Settings.DecimalSeparator)
                {
                    sb.Append(currentChar);
                    haveDecimalPoint = currentChar == Settings.DecimalSeparator;
                    currentChar = tokenizer.MoveNextChar();
                }

                var number = double.Parse(sb.ToString(), CultureInfo.InvariantCulture);
                if (tokenizer is Tokenizer<double> t)
                {
                    t.Number = number;
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

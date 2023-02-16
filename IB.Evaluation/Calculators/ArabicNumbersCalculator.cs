using IB.Evaluation.Calculators.Base;
using IB.Evaluation.Calculators.Settings;
using IB.Evaluation.Parsers;
using IB.Evaluation.Tokenizers;
using IB.Evaluation.Tokenizers.Enums;
using IB.Evaluation.Tokenizers.Selectors;

namespace IB.Evaluation.Calculators
{
    public class ArabicNumbersCalculator : ICalculator<double>
    {
        protected ArabicCalcSettings Settings { get; }

        public ArabicNumbersCalculator(ArabicCalcSettings settings)
        {
            Settings = settings;
        }

        public double Evaluate(string input)
        {
            using (var reader = new StringReader(input))
            {
                Stack<char> parenthesesChecker = new();

                var selectors = new EndFileSelector();
                selectors
                    .SetNext(new WhiteSpaceSelector())
                    .SetNext(new EndFileSelector())
                    .SetNext(new ParenthesesSelector(parenthesesChecker))
                    .SetNext(new ArabicNumbersSelector(Settings))
                    .SetNext(new MathOpertionsSelector())
                    .SetNext(new OpenParentheseChecker(parenthesesChecker));

                var tokenizer = new ArabicNumbersTokenizer(reader, selectors);

                while (tokenizer.CurrentToken != TokenTypes.EndOfFile)
                {
                    tokenizer.MoveNextToken();
                }

                var parser = new ArabicNumberParser(tokenizer.Tokens);
                return parser.Parse().Eval();
            }
        }
    }

}

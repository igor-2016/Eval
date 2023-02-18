using IB.Evaluation.Calculators.Base;
using IB.Evaluation.Parsers;
using IB.Evaluation.Parsers.Exceptions;
using IB.Evaluation.Tokenizers;
using IB.Evaluation.Tokenizers.Enums;
using IB.Evaluation.Tokenizers.Selectors;
using IB.Evaluation.Validators;

namespace IB.Evaluation.Calculators
{
    public class RomanNumbersCalculator : ICalculator<string>
    {
        public string Evaluate(string input)
        {
            using (var reader = new StringReader(input))
            {
                Stack<char> parentheseChecker = new();
                var selectors = new EndFileSelector();
                selectors
                    .SetNext(new WhiteSpaceSelector())
                    .SetNext(new EndFileSelector())
                    .SetNext(new ParenthesesSelector(parentheseChecker))
                    .SetNext(new RomanNumbersSelector())
                    .SetNext(new MathOpertionsSelector())
                    .SetNext(new OpenParentheseChecker(parentheseChecker));

                var tokenizer = new RomanNumbersTokenizer(reader, selectors);
                while (tokenizer.CurrentToken != TokenTypes.EndOfFile)
                {
                    tokenizer.MoveNextToken();
                }

                var parser = new RomanNumberParser(tokenizer.Tokens);
                return parser.Parse().Eval();
            }
        }
    }

}
